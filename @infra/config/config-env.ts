import * as fs from "fs";
import * as path from "path";

export type AwsRegion = "us-east-1" | "us-west-2";

export interface IConfigEnv {
  account: string;
  region: AwsRegion;
  environment: string;
  client: string;
  project: string;
  partner: string;
}

export class ConfigEnv {
  readonly config: IConfigEnv;

  constructor() {
    const environment = process.env.STACK_ENVIRONMENT || "dev";
    if (!environment) {
      throw new Error("Missing STACK_ENVIRONMENT environment variable");
    }
    if (process.env.AWS_ACCOUNT && process.env.AWS_REGION && process.env.CLIENT_NAME && process.env.PROJECT_NAME && process.env.PARTNER_NAME) {
      this.config = {
        account: process.env.AWS_ACCOUNT,
        region: process.env.AWS_REGION as AwsRegion,
        environment: environment,
        client: process.env.CLIENT_NAME,
        project: process.env.PROJECT_NAME,
        partner: process.env.PARTNER_NAME
      };
    } else {
      try {
        const configFile = fs.readFileSync(
          path.join("config", `${environment}.json`),
          {
            encoding: "utf-8",
          }
        );
        this.config = JSON.parse(configFile);
      } catch (error) {
        throw new Error(
          `Failed to load config for environment ${environment}: ${error}`
        );
      }
    }
  }
}
