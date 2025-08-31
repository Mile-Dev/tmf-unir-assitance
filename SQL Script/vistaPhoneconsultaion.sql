CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `admin`@`%` 
    SQL SECURITY DEFINER
VIEW `VwPhoneConsultationEvents` AS
    SELECT 
        `e`.`Id` AS `Id`,
        `vs`.`Id` AS `VoucherStatusId`,
        `es`.`Id` AS `EventStatusId`,
        `v`.`Name` AS `Voucher`,
        `c`.`Names` AS `Name`,
        `c`.`LastNames` AS `LastName`,
        `et`.`Name` AS `EventType`,
        `es`.`Name` AS `EventStatus`, /* Falta identificar he implementar este campo tabla PhoneConsultation  */
        `vs`.`Name` AS `VoucherStatus`,
        `e`.`CreatedAt` AS `EventStart`,
        `e`.`EndDate` AS `EventEnd`,
        'Front Agent Last' AS `LastUpdateBy`, /* Falta identificar he implementar este campo tener en cuenta tabla PhoneConsultation*/ 
        'Front Agent' AS `CreatedBy`, /* Falta identificar he implementar este campo tabla PhoneConsultation*/
        'Andres Plata' AS `LastDoctorAssigned`, /* Falta identificar he implementar este campo tabla PhoneConsultation*/
		CAST(NOW() AS DATE) AS `LastPhoneConsultation` /* Falta identificar he implementar este campo tabla PhoneConsultation */
		FROM
        (((((`Events` `e`
        JOIN `Vouchers` `v` ON ((`e`.`VoucherId` = `v`.`Id`)))
        JOIN `VoucherStatus` `vs` ON ((`v`.`VoucherStatusId` = `vs`.`Id`)))
        JOIN `GeneralTypes` `et` ON ((`e`.`GeneralTypesId` = `et`.`Id`)))
        JOIN `EventStatus` `es` ON ((`e`.`EventStatusId` = `es`.`Id`)))
        JOIN `CustomerTrip` `c` ON ((`e`.`CustomerTripId` = `c`.`Id`)))