CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `admin`@`%` 
    SQL SECURITY DEFINER
VIEW `VwEventDetail` AS
    SELECT 
        `e`.`Id` AS `Id`,
        CONCAT_WS('-', 'BT', `e`.`Id`) AS `CodeEvent`,
        `v`.`Name` AS `Voucher`,
        `v`.`Plan` AS `PlanVoucher`,
        `v`.`IssueName` AS `IssueBy`,
        `v`.`DateOfIssue` AS `DateOfIssueVoucher`,
        `v`.`StartDate` AS `StartDateVoucher`,
        `v`.`EndDate` AS `EndDateVoucher`,
        (TO_DAYS(CURDATE()) - TO_DAYS(`v`.`EndDate`)) AS `MissingDays`,
        `vs`.`Name` AS `StatusVoucher`,
        `v`.`Destination` AS `DestinationVoucher`,
        `ci`.`typeDocument` AS `TypeIdentification`,
        `ci`.`identification` AS `Identification`,
        `ci`.`email` AS `Email`,
        `ci`.`mobile` AS `Mobile`,
        IF(`ct`.`Gender`, 'Male', 'Female') AS `Gender`,
        `ct`.`DateOfBirth` AS `DateOfBirth`,
        `ct`.`CountryOfBirth` AS `CountryOfBirth`,
        `ct`.`Names` AS `Names`,
        `ct`.`LastNames` AS `LastNames`,
        CONCAT(COALESCE(`ct`.`Names`, ''),
                ' ',
                COALESCE(`ct`.`LastNames`, '')) AS `FullName`,
        `et`.`Name` AS `EventType`,
        `es`.`Name` AS `EventStatus`,
        `n`.`NameUser` AS `AssignedTo`,
        `e`.`CreatedAt` AS `EventStart`,
        `e`.`EndDate` AS `EventEnd`
    FROM
        (((((((`Events` `e`
        JOIN `Voucher` `v` ON ((`e`.`VoucherId` = `v`.`Id`)))
        JOIN `VoucherStatus` `vs` ON ((`v`.`VoucherStatusId` = `vs`.`Id`)))
        JOIN `GeneralType` `et` ON ((`e`.`GeneralTypesId` = `et`.`Id`)))
        JOIN `EventStatus` `es` ON ((`e`.`EventStatusId` = `es`.`Id`)))
        JOIN `CustomerTrip` `ct` ON ((`e`.`CustomerTripId` = `ct`.`Id`)))
        LEFT JOIN (SELECT 
            `ctp`.`Id` AS `CustomerTripId`,
                MAX((CASE
                    WHEN (`gt`.`Id` = 3) THEN `ct`.`Value`
                END)) AS `email`,
                MAX((CASE
                    WHEN (`gt`.`Id` = 5) THEN `ct`.`Value`
                END)) AS `mobile`,
                `ci`.`typeDocument` AS `typeDocument`,
                `ci`.`identification` AS `identification`
        FROM
            ((((`ContactInformation` `ct`
        JOIN `GeneralType` `gt` ON ((`gt`.`Id` = `ct`.`GeneralTypesId`)))
        JOIN `CustomerTrip` `ctp` ON ((`ct`.`CustomerTripId` = `ctp`.`Id`)))
        JOIN `Category` `c` ON ((`gt`.`CategoriesId` = `c`.`Id`)))
        LEFT JOIN (SELECT 
            `ct`.`CustomerTripId` AS `CustomerTripId`,
                `gt`.`Name` AS `typeDocument`,
                `ct`.`Value` AS `identification`
        FROM
            (`ContactInformation` `ct`
        JOIN `GeneralType` `gt` ON (((`gt`.`Id` = `ct`.`GeneralTypesId`)
            AND (`gt`.`CategoriesId` = 1))))) `ci` ON ((`ctp`.`Id` = `ci`.`CustomerTripId`)))
        WHERE
            (`gt`.`Id` IN (3 , 5))
        GROUP BY `ctp`.`Id` , `ci`.`typeDocument`) `ci` ON ((`e`.`CustomerTripId` = `ci`.`CustomerTripId`)))
        LEFT JOIN `EventNote` `n` ON (((`e`.`Id` = `n`.`EventId`)
            AND (`n`.`NoteType` = 'AssignedTo'))))
    ORDER BY `e`.`Id`