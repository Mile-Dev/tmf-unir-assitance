export class SubnetGroupListNotFoundException extends Error {
    constructor(mensaje : string) {
      super(mensaje);
      this.name = 'SubnetGroupListNotFoundException';
    }
  }



export class ZoneNotFoundException extends Error {
  constructor(mensaje : string) {
    super(mensaje);
    this.name = 'ZoneNotFoundException';
  }
}