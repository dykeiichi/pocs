CREATE DATABASE  IF NOT EXISTS `UsersDatabase`;
USE `UsersDatabase`;

DROP TABLE IF EXISTS `User`;

CREATE TABLE `User` (
  `username` varchar(45) COLLATE latin1_spanish_ci NOT NULL,
  `password` tinyblob NOT NULL,
  `salt` tinyblob NOT NULL,
  `password_jwt` blob NOT NULL,
  `secret_totp` blob NOT NULL,
  PRIMARY KEY (`username`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  UNIQUE KEY `salt_UNIQUE` (`salt`(16)),
  UNIQUE KEY `secret_totp_UNIQUE` (`secret_totp`(16))
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;


DROP TABLE IF EXISTS `vw_read_user`;
DROP VIEW IF EXISTS `vw_read_user`;

CREATE
	ALGORITHM=UNDEFINED
	DEFINER=`root`@`localhost`
	SQL SECURITY DEFINER
VIEW `UsersDatabase`.`vw_read_user` AS
	select
		`UsersDatabase`.`User`.`username` AS `Username`,
		hex(`UsersDatabase`.`User`.`password`) AS `Password`,
		hex(`UsersDatabase`.`User`.`salt`) AS `Salt`,
		hex(`UsersDatabase`.`User`.`password_jwt`) AS `Password_jwt`,
		hex(`UsersDatabase`.`User`.`secret_totp`) AS `Secret_totp`
	from
		`UsersDatabase`.`User`;


DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_create_user`(IN chvUser VARCHAR(45), IN chvPassword VARCHAR(256), IN chvSalt VARCHAR(32), IN chvPasswordJWT VARCHAR(512), IN chvTOTP VARCHAR(64))
BEGIN
	INSERT INTO `UsersDatabase`.`User`
	(`username`,
	`password`,
	`salt`,
	`password_jwt`,
	`secret_totp`)
	VALUES
	(chvUser,
	UNHEX(chvPassword),
	UNHEX(chvSalt),
	UNHEX(chvPasswordJWT),
	UNHEX(chvTOTP));
END ;;
DELIMITER ;