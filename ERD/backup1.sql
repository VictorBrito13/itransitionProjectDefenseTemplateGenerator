-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: itransition_template_manager
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20241012004435_version_1','8.0.10'),('20241012011427_version_2','8.0.10'),('20241012012456_version_3','8.0.10'),('20241012022156_version_4','8.0.10'),('20241012023604_version_4.1','8.0.10'),('20241012024133_version_4.2','8.0.10'),('20241012025840_version_4.3','8.0.10'),('20241012030007_version_4.4','8.0.10'),('20241012030420_version_4.5','8.0.10'),('20241012030739_version_4.6','8.0.10'),('20241014040228_version_4.7','8.0.10'),('20241015015434_QuestionType_defined','8.0.10'),('20241015020447_ParseQuetionTypeToString','8.0.10'),('20241015041338_nullable_values','8.0.10'),('20241016042452_realtion_topic_template','8.0.10'),('20241016043026_realtion_topic_template_2.0','8.0.10'),('20241020195444_dates_as_strings','8.0.10'),('20241020195923_dates_as_strings_2.0','8.0.10'),('20241020231705_comment_table_update','8.0.10');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admins` (
  `UserId` bigint unsigned NOT NULL,
  `TemplateId` bigint unsigned NOT NULL,
  PRIMARY KEY (`UserId`,`TemplateId`),
  KEY `IX_Admins_TemplateId` (`TemplateId`),
  CONSTRAINT `FK_Admins_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Admins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `admins` VALUES (1,2),(1,14),(2,14),(3,14),(4,14),(6,14),(7,14),(7,15),(7,17),(1,18),(7,19),(7,20),(7,21);
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `UserId` bigint unsigned NOT NULL,
  `TemplateId` bigint unsigned NOT NULL,
  `CommentString` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`TemplateId`),
  KEY `IX_Comments_TemplateId` (`TemplateId`),
  CONSTRAINT `FK_Comments_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `likes`
--

DROP TABLE IF EXISTS `likes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `likes` (
  `TemplateId` bigint unsigned NOT NULL,
  `UserId` bigint unsigned NOT NULL,
  PRIMARY KEY (`TemplateId`,`UserId`),
  KEY `IX_Likes_UserId` (`UserId`),
  CONSTRAINT `FK_Likes_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Likes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `likes`
--

LOCK TABLES `likes` WRITE;
/*!40000 ALTER TABLE `likes` DISABLE KEYS */;
INSERT INTO `likes` VALUES (14,7),(15,7),(17,7);
/*!40000 ALTER TABLE `likes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionoptions`
--

DROP TABLE IF EXISTS `questionoptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionoptions` (
  `QuestionOptionId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Option` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `QuestionId` bigint unsigned NOT NULL,
  PRIMARY KEY (`QuestionOptionId`),
  KEY `IX_QuestionOptions_QuestionId` (`QuestionId`),
  CONSTRAINT `FK_QuestionOptions_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionoptions`
--

LOCK TABLES `questionoptions` WRITE;
/*!40000 ALTER TABLE `questionoptions` DISABLE KEYS */;
/*!40000 ALTER TABLE `questionoptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questions` (
  `QuestionId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `QuestionString` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TemplateId` bigint unsigned NOT NULL,
  `QuestionType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`QuestionId`),
  KEY `IX_Questions_TemplateId` (`TemplateId`),
  CONSTRAINT `FK_Questions_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questions`
--

LOCK TABLES `questions` WRITE;
/*!40000 ALTER TABLE `questions` DISABLE KEYS */;
INSERT INTO `questions` VALUES (100,'Add a label',17,'singleLineString'),(101,'Tu nombre',18,'singleLineString'),(102,'Name',19,'singleLineString'),(103,'Last Name',19,'singleLineString'),(104,'Are you an adult?',19,'checkBox'),(105,'Add a label',20,'singleLineString'),(106,'Add a label',21,'checkBox');
/*!40000 ALTER TABLE `questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `responses`
--

DROP TABLE IF EXISTS `responses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `responses` (
  `ResponseId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Date` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ResponseString` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `QuestionId` bigint unsigned NOT NULL DEFAULT '0',
  `UserId` bigint unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ResponseId`),
  UNIQUE KEY `IX_Responses_QuestionId` (`QuestionId`),
  KEY `IX_Responses_UserId` (`UserId`),
  CONSTRAINT `FK_Responses_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Responses_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `responses`
--

LOCK TABLES `responses` WRITE;
/*!40000 ALTER TABLE `responses` DISABLE KEYS */;
INSERT INTO `responses` VALUES (1,'2024-12-30 18:36:18.810577','',102,7),(2,'2024-12-30 18:36:18.811217','',103,7),(3,'2024-12-30 18:36:18.811219','on',104,7);
/*!40000 ALTER TABLE `responses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tag_has_templates`
--

DROP TABLE IF EXISTS `tag_has_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tag_has_templates` (
  `TemplateId` bigint unsigned NOT NULL,
  `TagId` bigint unsigned NOT NULL,
  PRIMARY KEY (`TemplateId`,`TagId`),
  KEY `IX_Tag_has_templates_TagId` (`TagId`),
  CONSTRAINT `FK_Tag_has_templates_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `tags` (`TagId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Tag_has_templates_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tag_has_templates`
--

LOCK TABLES `tag_has_templates` WRITE;
/*!40000 ALTER TABLE `tag_has_templates` DISABLE KEYS */;
/*!40000 ALTER TABLE `tag_has_templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tags`
--

DROP TABLE IF EXISTS `tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tags` (
  `TagId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`TagId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags`
--

LOCK TABLES `tags` WRITE;
/*!40000 ALTER TABLE `tags` DISABLE KEYS */;
/*!40000 ALTER TABLE `tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `templates`
--

DROP TABLE IF EXISTS `templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `templates` (
  `TemplateId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Image_url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsPublic` tinyint(1) NOT NULL,
  `TopicId` bigint unsigned NOT NULL,
  PRIMARY KEY (`TemplateId`),
  KEY `IX_Templates_TopicId` (`TopicId`),
  FULLTEXT KEY `idx_fulltext_title_description` (`Title`,`Description`),
  CONSTRAINT `FK_Templates_Topics_TopicId` FOREIGN KEY (`TopicId`) REFERENCES `topics` (`TopicId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `templates`
--

LOCK TABLES `templates` WRITE;
/*!40000 ALTER TABLE `templates` DISABLE KEYS */;
INSERT INTO `templates` VALUES (2,'Test 1 template','Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.','default.png',1,1),(8,'Template title','Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.','default.png',1,1),(9,'Template title','Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.','default.png',1,1),(10,'Template title','Template Description','default.png',1,1),(11,'Template title','Template Description','default.png',1,1),(12,'Template title','Template Description','default.png',1,1),(13,'Template title','Template Description','default.png',1,1),(14,'Template title','Template Description','default.png',0,1),(15,'Template title','Template Description','default.png',1,1),(17,'Template','Template Description','default.png',0,1),(18,'Template title','Template Description','default.png',1,1),(19,'Template title','Template Description','default.png',1,1),(20,'Template title','Template Description','default.png',1,1),(21,'Template title','Template Description','default.png',1,1);
/*!40000 ALTER TABLE `templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `topics`
--

DROP TABLE IF EXISTS `topics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `topics` (
  `TopicId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`TopicId`),
  UNIQUE KEY `IX_Topics_TopicId_Name` (`TopicId`,`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `topics`
--

LOCK TABLES `topics` WRITE;
/*!40000 ALTER TABLE `topics` DISABLE KEYS */;
INSERT INTO `topics` VALUES (1,'Education');
/*!40000 ALTER TABLE `topics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userallowedtoanswers`
--

DROP TABLE IF EXISTS `userallowedtoanswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userallowedtoanswers` (
  `UserId` bigint unsigned NOT NULL,
  `TemplateId` bigint unsigned NOT NULL,
  PRIMARY KEY (`UserId`,`TemplateId`),
  KEY `IX_UserAllowedToAnswers_TemplateId` (`TemplateId`),
  CONSTRAINT `FK_UserAllowedToAnswers_Templates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `templates` (`TemplateId`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserAllowedToAnswers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userallowedtoanswers`
--

LOCK TABLES `userallowedtoanswers` WRITE;
/*!40000 ALTER TABLE `userallowedtoanswers` DISABLE KEYS */;
INSERT INTO `userallowedtoanswers` VALUES (6,14),(7,14);
/*!40000 ALTER TABLE `userallowedtoanswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` bigint unsigned NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `IDX_User_Email` (`Email`),
  FULLTEXT KEY `idx_fulltext` (`Username`,`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'test1','test1@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(2,'test2','test2@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(3,'test3','test3@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(4,'test4','test4@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(5,'test5','test5@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(6,'test6','test6@test.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(7,'root','root@root.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),(8,'a','a@a.com','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-30 18:50:25
