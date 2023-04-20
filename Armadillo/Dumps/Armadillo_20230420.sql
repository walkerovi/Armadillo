CREATE DATABASE  IF NOT EXISTS `armadillo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `armadillo`;
-- MySQL dump 10.13  Distrib 8.0.32, for Win64 (x86_64)
--
-- Host: localhost    Database: armadillo
-- ------------------------------------------------------
-- Server version	8.0.32

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
-- Table structure for table `campo`
--

DROP TABLE IF EXISTS `campo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `campo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Indice` int NOT NULL,
  `IdTipo` int NOT NULL,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IdHoja` int NOT NULL,
  `Calculo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Campo_IdHoja` (`IdHoja`),
  KEY `IX_Campo_IdTipo` (`IdTipo`),
  CONSTRAINT `FK_Campo_Hoja_IdHoja` FOREIGN KEY (`IdHoja`) REFERENCES `hoja` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Campo_Tipo_IdTipo` FOREIGN KEY (`IdTipo`) REFERENCES `tipo` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campo`
--

LOCK TABLES `campo` WRITE;
/*!40000 ALTER TABLE `campo` DISABLE KEYS */;
INSERT INTO `campo` VALUES (31,1,1,'Nombre',11,'0'),(32,2,1,'Apellido',11,'0'),(33,1,1,'Tipo',12,'0'),(34,2,1,'Número',12,'0'),(36,3,6,'Lista de Teléfonos',11,'12'),(37,3,7,'Datos',12,'11'),(38,4,8,'Nombre',12,'8');
/*!40000 ALTER TABLE `campo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dato`
--

DROP TABLE IF EXISTS `dato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dato` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `NoFila` int NOT NULL,
  `Indice` int NOT NULL,
  `IdCampo` int NOT NULL,
  `Valor` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Dato_IdCampo` (`IdCampo`),
  CONSTRAINT `FK_Dato_Campo_IdCampo` FOREIGN KEY (`IdCampo`) REFERENCES `campo` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=90 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dato`
--

LOCK TABLES `dato` WRITE;
/*!40000 ALTER TABLE `dato` DISABLE KEYS */;
INSERT INTO `dato` VALUES (69,1,1,31,'Catalino2'),(70,1,2,32,'Miramontes'),(71,1,3,36,'71'),(72,2,1,31,'Fermino'),(73,2,2,32,'Villalta4'),(74,2,3,36,'74'),(75,1,1,33,'Trabajo'),(76,1,2,34,'5555555'),(78,2,1,33,'Personal'),(79,2,2,34,'77777'),(81,3,1,33,'Negocio'),(82,3,2,34,'11111111'),(84,4,1,33,'hogar'),(85,4,2,34,'8888888'),(87,5,1,33,'Privado'),(88,5,2,34,'333333'),(89,5,3,37,'11,1');
/*!40000 ALTER TABLE `dato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hoja`
--

DROP TABLE IF EXISTS `hoja`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hoja` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdPrograma` int NOT NULL,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Hoja_IdPrograma` (`IdPrograma`),
  CONSTRAINT `FK_Hoja_Programa_IdPrograma` FOREIGN KEY (`IdPrograma`) REFERENCES `programa` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hoja`
--

LOCK TABLES `hoja` WRITE;
/*!40000 ALTER TABLE `hoja` DISABLE KEYS */;
INSERT INTO `hoja` VALUES (11,7,'Datos Generales','datos que no cambia'),(12,7,'Teléfonos','teléfonos de clientes');
/*!40000 ALTER TABLE `hoja` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `programa`
--

DROP TABLE IF EXISTS `programa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `programa` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Descripcion` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programa`
--

LOCK TABLES `programa` WRITE;
/*!40000 ALTER TABLE `programa` DISABLE KEYS */;
INSERT INTO `programa` VALUES (1,'Presupuesto','Para el hogar'),(7,'Mis Clientes','información de clientes');
/*!40000 ALTER TABLE `programa` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo`
--

DROP TABLE IF EXISTS `tipo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo`
--

LOCK TABLES `tipo` WRITE;
/*!40000 ALTER TABLE `tipo` DISABLE KEYS */;
INSERT INTO `tipo` VALUES (1,'Texto'),(2,'Número Entero'),(3,'Número Decimal'),(4,'Fecha'),(5,'Cálculo'),(6,'Lista'),(7,'Detalle'),(8,'Campo Foráneo');
/*!40000 ALTER TABLE `tipo` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-04-20 16:42:03
