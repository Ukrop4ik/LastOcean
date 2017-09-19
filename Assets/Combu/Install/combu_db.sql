-- phpMyAdmin SQL Dump
-- version 4.5.4.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Oct 23, 2016 at 12:39 PM
-- Server version: 5.7.15
-- PHP Version: 5.6.24

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `combu`
--

-- --------------------------------------------------------

--
-- Table structure for table `CB_Account`
--

CREATE TABLE IF NOT EXISTS `CB_Account` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `Password` varchar(45) NOT NULL,
  `LastLoginIp` varchar(255) DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `ActivationCode` varchar(10) DEFAULT NULL,
  `ChangePwdCode` varchar(10) DEFAULT NULL,
  `Enabled` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AccountUsername` (`Username`),
  KEY `AccountPassword` (`Password`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Account_Platform`
--

CREATE TABLE IF NOT EXISTS `CB_Account_Platform` (
  `IdAccount` bigint(20) NOT NULL,
  `PlatformKey` varchar(45) NOT NULL,
  `PlatformId` varchar(255) NOT NULL,
  PRIMARY KEY (`IdAccount`,`PlatformId`,`PlatformKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Achievement`
--

CREATE TABLE IF NOT EXISTS `CB_Achievement` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) CHARACTER SET latin1 NOT NULL,
  `Description` varchar(255) CHARACTER SET latin1 DEFAULT NULL,
  `UniqueRecords` tinyint(1) NOT NULL DEFAULT '0',
  `Position` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Achievement_User`
--

CREATE TABLE IF NOT EXISTS `CB_Achievement_User` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAchievement` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `Progress` smallint(6) NOT NULL,
  `LastUpdated` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `AchievementUser` (`IdAchievement`),
  KEY `UserAchievement` (`IdAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_AdminAccount`
--

CREATE TABLE IF NOT EXISTS `CB_AdminAccount` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `Password` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `AdminAccountUsername` (`Username`),
  KEY `AdminAccountPassword` (`Password`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_CustomData`
--

CREATE TABLE IF NOT EXISTS `CB_CustomData` (
  `IdAccount` bigint(20) NOT NULL,
  `DataKey` varchar(45) NOT NULL,
  `DataValue` text,
  PRIMARY KEY (`IdAccount`,`DataKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Friend`
--

CREATE TABLE IF NOT EXISTS `CB_Friend` (
  `IdAccount` bigint(20) NOT NULL,
  `IdFriend` bigint(20) NOT NULL,
  `State` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdAccount`,`IdFriend`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Inventory`
--

CREATE TABLE IF NOT EXISTS `CB_Inventory` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAccount` bigint(20) NOT NULL,
  `Name` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Quantity` int(11) NOT NULL,
  `CustomData` text CHARACTER SET latin1,
  PRIMARY KEY (`Id`),
  KEY `InventoryAccount` (`IdAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Issue`
--

CREATE TABLE IF NOT EXISTS `CB_Issue` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAccount` bigint(20) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` longtext NOT NULL,
  `UrlLog` varchar(255) DEFAULT NULL,
  `UrlScreenshot` varchar(255) DEFAULT NULL,
  `IssueDate` datetime NOT NULL,
  `IssueIp` varchar(255) DEFAULT NULL,
  `IssueState` tinyint(1) NOT NULL DEFAULT '0',
  `CustomData` longtext,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IssueAccount` (`IdAccount`,`IssueDate`),
  KEY `IssueState` (`IssueDate`,`IssueState`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_LeaderBoard`
--

CREATE TABLE IF NOT EXISTS `CB_LeaderBoard` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `UniqueRecords` tinyint(1) NOT NULL DEFAULT '0',
  `ValueType` tinyint(1) NOT NULL DEFAULT '0',
  `OrderType` tinyint(1) NOT NULL DEFAULT '0',
  `AllowAnonymous` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_LeaderBoard_User`
--

CREATE TABLE IF NOT EXISTS `CB_LeaderBoard_User` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdLeaderboard` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `ValueInt` int(11) NOT NULL DEFAULT '0',
  `ValueFloat` decimal(10,6) NOT NULL DEFAULT '0.000000',
  `LastUpdated` datetime DEFAULT NULL,
  `Username` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `LeaderboardScoreValue` (`IdLeaderboard`,`ValueInt`,`ValueFloat`) USING BTREE,
  KEY `LeaderboardAccount` (`IdAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Mail`
--

CREATE TABLE IF NOT EXISTS `CB_Mail` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SendDate` datetime NOT NULL,
  `ReadDate` datetime DEFAULT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `IdGroup` bigint(20) NOT NULL,
  `IdSender` bigint(20) NOT NULL,
  `IsPublic` tinyint(1) NOT NULL,
  `Subject` varchar(255) NOT NULL,
  `Message` longtext NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `MailDate` (`SendDate`,`IdAccount`,`IdSender`),
  KEY `MailSender` (`IdSender`,`SendDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Match`
--

CREATE TABLE IF NOT EXISTS `CB_Match` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdTournament` bigint(20) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `DateCreation` datetime NOT NULL,
  `DateExpire` datetime DEFAULT NULL,
  `Rounds` int(2) NOT NULL,
  `Finished` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Match_Finished` (`Finished`,`DateCreation`,`DateExpire`),
  KEY `IDX_Match_Tournament` (`IdTournament`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Match_Account`
--

CREATE TABLE IF NOT EXISTS `CB_Match_Account` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdMatch` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `CustomData` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Account_User_idx` (`IdAccount`),
  KEY `FK_Account_Match_idx` (`IdMatch`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Match_CustomData`
--

CREATE TABLE IF NOT EXISTS `CB_Match_CustomData` (
  `IdMatch` bigint(20) NOT NULL,
  `DataKey` varchar(45) NOT NULL,
  `DataValue` text,
  PRIMARY KEY (`IdMatch`,`DataKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Match_Round`
--

CREATE TABLE IF NOT EXISTS `CB_Match_Round` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdMatchAccount` bigint(20) NOT NULL,
  `Score` decimal(20,5) NOT NULL DEFAULT '0.00000',
  `DateScore` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Round_Match_idx` (`IdMatchAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_News`
--

CREATE TABLE IF NOT EXISTS `CB_News` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAdminAccount` bigint(20) NOT NULL,
  `PublishDate` datetime NOT NULL,
  `Subject` varchar(255) NOT NULL,
  `Message` longtext NOT NULL,
  `Url` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Newsletter`
--

CREATE TABLE IF NOT EXISTS `CB_Newsletter` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAccount` bigint(20) NOT NULL,
  `Subject` varchar(255) NOT NULL,
  `Body` longtext NOT NULL,
  `IsHtml` tinyint(1) NOT NULL,
  `DateCreation` datetime NOT NULL,
  `DateSent` datetime DEFAULT NULL,
  `Status` tinyint(1) DEFAULT NULL,
  `LogMessage` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_NewsletterLog`
--

CREATE TABLE IF NOT EXISTS `CB_NewsletterLog` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdNewsletter` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `Sent` tinyint(1) NOT NULL,
  `DateCreation` datetime NOT NULL,
  `Message` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_ServerSettings`
--

CREATE TABLE IF NOT EXISTS `CB_ServerSettings` (
  `DataKey` varchar(45) NOT NULL,
  `DataValue` text,
  PRIMARY KEY (`DataKey`),
  UNIQUE KEY `DataKey_UNIQUE` (`DataKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Session`
--

CREATE TABLE IF NOT EXISTS `CB_Session` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAccount` bigint(20) NOT NULL,
  `GUID` varchar(45) NOT NULL,
  `LastActionDate` datetime NOT NULL,
  `LastActionIP` varchar(45) NOT NULL,
  `SignatureTimestamp` varchar(45) DEFAULT NULL,
  `Expire` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserSession` (`IdAccount`,`GUID`,`LastActionDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Tournament`
--

CREATE TABLE IF NOT EXISTS `CB_Tournament` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdOwner` bigint(20) NOT NULL,
  `DateCreation` datetime DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Finished` tinyint(1) NOT NULL,
  `DateFinished` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Tournament_Finished` (`Finished`,`DateCreation`,`DateFinished`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_Tournament_CustomData`
--

CREATE TABLE IF NOT EXISTS `CB_Tournament_CustomData` (
  `IdTournament` bigint(20) NOT NULL,
  `DataKey` varchar(45) NOT NULL,
  `DataValue` text,
  PRIMARY KEY (`IdTournament`,`DataKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_UserFile`
--

CREATE TABLE IF NOT EXISTS `CB_UserFile` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdAccount` bigint(20) NOT NULL,
  `Name` varchar(45) NOT NULL,
  `Url` varchar(255) CHARACTER SET latin1 NOT NULL,
  `ShareType` tinyint(1) NOT NULL,
  `Likes` int(11) NOT NULL,
  `Views` int(11) NOT NULL,
  `CustomData` text,
  PRIMARY KEY (`Id`),
  KEY `UserFileAccount` (`IdAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_UserFilesActivity`
--

CREATE TABLE IF NOT EXISTS `CB_UserFilesActivity` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdFile` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  `Likes` tinyint(1) NOT NULL,
  `Views` int(11) NOT NULL,
  `LastActivity` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_UserGroup`
--

CREATE TABLE IF NOT EXISTS `CB_UserGroup` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `IdOwner` bigint(20) NOT NULL,
  `Public` tinyint(1) NOT NULL,
  `CustomData` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserGroupOwner` (`IdOwner`,`Public`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `CB_UserGroupAccount`
--

CREATE TABLE IF NOT EXISTS `CB_UserGroupAccount` (
  `IdGroup` bigint(20) NOT NULL,
  `IdAccount` bigint(20) NOT NULL,
  PRIMARY KEY (`IdGroup`,`IdAccount`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
