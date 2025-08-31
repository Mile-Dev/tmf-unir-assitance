CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `admin`@`%` 
    SQL SECURITY DEFINER
VIEW `VwPhoneConsultationEvent` AS
    SELECT 
        `e`.`Id` AS `Id`,
        CONCAT_WS('-', 'BT', `e`.`Id`) AS `CodeEvent`,
        `vs`.`Id` AS `VoucherStatusId`,
        `es`.`Id` AS `EventStatusId`,
        `v`.`Name` AS `Voucher`,
        `c`.`Names` AS `Name`,
        `c`.`LastNames` AS `LastName`,
        `et`.`Name` AS `EventType`,
        `es`.`Name` AS `EventStatus`,
        `vs`.`Name` AS `VoucherStatus`,
        `e`.`CreatedAt` AS `EventStart`,
        `e`.`EndDate` AS `EventEnd`,
        'Front Agent Last' AS `LastUpdateBy`,
        'Front Agent' AS `CreatedBy`,
        'Andres Plata' AS `LastDoctorAssigned`,
        CAST(NOW() AS DATE) AS `LastPhoneConsultation`,
        `e`.`UpdatedAt` AS `UpdatedAt`
    FROM
        (((((`Event` `e`
        JOIN `Voucher` `v` ON ((`e`.`VoucherId` = `v`.`Id`)))
        JOIN `VoucherStatus` `vs` ON ((`v`.`VoucherStatusId` = `vs`.`Id`)))
        JOIN `GeneralType` `et` ON ((`e`.`GeneralTypesId` = `et`.`Id`)))
        JOIN `EventStatus` `es` ON ((`e`.`EventStatusId` = `es`.`Id`)))
        JOIN `CustomerTrip` `c` ON ((`e`.`CustomerTripId` = `c`.`Id`)))