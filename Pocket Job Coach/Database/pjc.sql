/*
Navicat MySQL Data Transfer

Source Server         : Local
Source Server Version : 50703
Source Host           : localhost:3306
Source Database       : pjc

Target Server Type    : MYSQL
Target Server Version : 50703
File Encoding         : 65001

Date: 2015-01-26 11:15:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `job`
-- ----------------------------
DROP TABLE IF EXISTS `job`;
CREATE TABLE `job` (
  `jobID` int(10) NOT NULL,
  `jobTitle` varchar(25) NOT NULL,
  PRIMARY KEY (`jobID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of job
-- ----------------------------

-- ----------------------------
-- Table structure for `jobtask`
-- ----------------------------
DROP TABLE IF EXISTS `jobtask`;
CREATE TABLE `jobtask` (
  `jobID` int(10) NOT NULL,
  `taskID` int(10) NOT NULL,
  PRIMARY KEY (`jobID`,`taskID`),
  KEY `jobtask_taskID_FK` (`taskID`),
  CONSTRAINT `jobtask_taskID_FK` FOREIGN KEY (`taskID`) REFERENCES `task` (`taskID`) ON UPDATE CASCADE,
  CONSTRAINT `jobtask_jobID_FK` FOREIGN KEY (`jobID`) REFERENCES `job` (`jobID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of jobtask
-- ----------------------------

-- ----------------------------
-- Table structure for `prompt`
-- ----------------------------
DROP TABLE IF EXISTS `prompt`;
CREATE TABLE `prompt` (
  `promptID` int(10) NOT NULL,
  `typeID` int(10) NOT NULL,
  `taskID` int(10) NOT NULL,
  `title` varchar(25) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`promptID`),
  KEY `prompt_typeID_FK` (`typeID`),
  KEY `prompt_taskID_FK` (`taskID`),
  CONSTRAINT `prompt_taskID_FK` FOREIGN KEY (`taskID`) REFERENCES `task` (`taskID`) ON UPDATE CASCADE,
  CONSTRAINT `prompt_typeID_FK` FOREIGN KEY (`typeID`) REFERENCES `prompttype` (`typeID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of prompt
-- ----------------------------

-- ----------------------------
-- Table structure for `prompttype`
-- ----------------------------
DROP TABLE IF EXISTS `prompttype`;
CREATE TABLE `prompttype` (
  `typeID` int(10) NOT NULL,
  `typeName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of prompttype
-- ----------------------------

-- ----------------------------
-- Table structure for `task`
-- ----------------------------
DROP TABLE IF EXISTS `task`;
CREATE TABLE `task` (
  `taskID` int(10) NOT NULL,
  `taskCategoryID` int(10) NOT NULL,
  `taskName` varchar(50) NOT NULL,
  `description` text NOT NULL,
  PRIMARY KEY (`taskID`),
  KEY `task_taskCategoryID_FK` (`taskCategoryID`),
  CONSTRAINT `task_taskCategoryID_FK` FOREIGN KEY (`taskCategoryID`) REFERENCES `taskcategory` (`categoryID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of task
-- ----------------------------

-- ----------------------------
-- Table structure for `taskcategory`
-- ----------------------------
DROP TABLE IF EXISTS `taskcategory`;
CREATE TABLE `taskcategory` (
  `categoryID` int(10) NOT NULL,
  `categoryName` varchar(25) NOT NULL,
  PRIMARY KEY (`categoryID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of taskcategory
-- ----------------------------

-- ----------------------------
-- Table structure for `user`
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `userID` int(10) NOT NULL,
  `firstName` varchar(25) NOT NULL,
  `lastName` varchar(25) NOT NULL,
  `address` varchar(50) NOT NULL,
  `city` varchar(40) NOT NULL,
  `state` varchar(2) NOT NULL,
  `homePhone` varchar(10) NOT NULL,
  `mobilePhone` varchar(10) NOT NULL,
  `workPhone` varchar(10) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `userTypeID` int(10) NOT NULL,
  PRIMARY KEY (`userID`),
  KEY `userTypeID_FK` (`userTypeID`),
  CONSTRAINT `userTypeID_FK` FOREIGN KEY (`userTypeID`) REFERENCES `usertype` (`userTypeID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------

-- ----------------------------
-- Table structure for `userjob`
-- ----------------------------
DROP TABLE IF EXISTS `userjob`;
CREATE TABLE `userjob` (
  `jobID` int(10) NOT NULL,
  `userID` int(10) NOT NULL,
  PRIMARY KEY (`jobID`,`userID`),
  KEY `userjob_userID_FK` (`userID`),
  CONSTRAINT `userjob_userID_FK` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `userjob_jobid_FK` FOREIGN KEY (`jobID`) REFERENCES `job` (`jobID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userjob
-- ----------------------------

-- ----------------------------
-- Table structure for `userrelation`
-- ----------------------------
DROP TABLE IF EXISTS `userrelation`;
CREATE TABLE `userrelation` (
  `userID` int(10) NOT NULL,
  `userID2` int(10) NOT NULL,
  PRIMARY KEY (`userID`,`userID2`),
  KEY `userID2Relation_FK` (`userID2`),
  CONSTRAINT `userIDRelation_FK` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `userID2Relation_FK` FOREIGN KEY (`userID2`) REFERENCES `user` (`userID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userrelation
-- ----------------------------

-- ----------------------------
-- Table structure for `usertask`
-- ----------------------------
DROP TABLE IF EXISTS `usertask`;
CREATE TABLE `usertask` (
  `userID` int(10) NOT NULL,
  `taskID` int(10) NOT NULL,
  `startTime` datetime DEFAULT NULL,
  `endTime` datetime DEFAULT NULL,
  `daysOfWeek` varchar(7) DEFAULT NULL,
  `sendNotification` tinyint(1) NOT NULL,
  `feedbackMessage` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`userID`,`taskID`),
  KEY `usertask_taskID_FK` (`taskID`),
  CONSTRAINT `usertask_taskID_FK` FOREIGN KEY (`taskID`) REFERENCES `task` (`taskID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `usertask_userID_FK` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usertask
-- ----------------------------

-- ----------------------------
-- Table structure for `usertaskprompt`
-- ----------------------------
DROP TABLE IF EXISTS `usertaskprompt`;
CREATE TABLE `usertaskprompt` (
  `userID` int(10) NOT NULL,
  `taskID` int(10) NOT NULL,
  `promptTypeID` int(10) NOT NULL,
  `promptLengthMin` int(2) DEFAULT NULL,
  PRIMARY KEY (`userID`,`taskID`,`promptTypeID`),
  KEY `usertaskprompt_taskID_FK` (`taskID`),
  KEY `usertaskprompt_promptTypeID_FK` (`promptTypeID`),
  CONSTRAINT `usertaskprompt_promptTypeID_FK` FOREIGN KEY (`promptTypeID`) REFERENCES `prompttype` (`typeID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `usertaskprompt_taskID_FK` FOREIGN KEY (`taskID`) REFERENCES `task` (`taskID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `usertaskprompt_userID_FK` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usertaskprompt
-- ----------------------------

-- ----------------------------
-- Table structure for `usertype`
-- ----------------------------
DROP TABLE IF EXISTS `usertype`;
CREATE TABLE `usertype` (
  `userTypeID` int(10) NOT NULL,
  `typeName` varchar(25) NOT NULL,
  PRIMARY KEY (`userTypeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usertype
-- ----------------------------
