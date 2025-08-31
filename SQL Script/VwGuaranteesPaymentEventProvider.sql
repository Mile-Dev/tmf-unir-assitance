CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `admin`@`%` 
    SQL SECURITY DEFINER
VIEW `VwGuaranteesPaymentEventProvider` AS
    SELECT 
        `gp`.`Id` AS `id`,
		`evp`.`Id` AS `EventProviderId`,
        `evp`.`EventId` AS `EventId`,
        `evp`.`EventProviderStatusId` AS `StatusEventProviderId`,
        `evps`.`Name` AS `NameStatusEventProvider`,
        `evp`.`Country` AS `Country`,
        `evp`.`City` AS `City`,
        `evp`.`CreatedAt` AS `CreatedAt`,
        `evp`.`EndDate` AS `EndDate`,
        `evp`.`ScheduledAppointment` AS `ScheduledAppointment`,
        `evp`.`Address` AS `Address`,
        `evp`.`NameProvider` AS `NameProvider`,
        `evp`.`TypeProvider` AS `TypeProvider`,
        `gp`.`AmountLocal` AS `AmountLocal`,
        `gp`.`AmountUsd` AS `AmountUsd`,
        `gp`.`TypeMoney` AS `TypeMoney`,
        `gp`.`ExchangeRate` AS `ExchangeRate`,
        `gp`.`DeductibleAmountLocal` AS `DeductibleAmountLocal`,
        `gp`.`DeductibleAmountUsd` AS `DeductibleAmountUsd`,
        `gp`.`GuaranteePaymentStatusId` AS `GuaranteePaymentStatusId`,
        `gps`.`Name` AS `NameStatusGuaranteePayment`
    FROM
        (((`EventProvider` `evp`
        JOIN `GuaranteePayment` `gp` ON ((`evp`.`Id` = `gp`.`EventProviderId`)))
        JOIN `EventProviderStatus` `evps` ON ((`evp`.`EventProviderStatusId` = `evps`.`Id`)))
        JOIN `GuaranteePaymentStatus` `gps` ON ((`gps`.`Id` = `gp`.`GuaranteePaymentStatusId`)))