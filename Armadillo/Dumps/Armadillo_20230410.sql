CREATE DATABASE  IF NOT EXISTS `armadillo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `armadillo`;
-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: armadillo
-- ------------------------------------------------------
-- Server version	8.0.27

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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campo`
--

LOCK TABLES `campo` WRITE;
/*!40000 ALTER TABLE `campo` DISABLE KEYS */;
INSERT INTO `campo` VALUES (1,1,2,'Cantidad',1,'0'),(2,2,1,'Nombre',1,'0'),(3,3,3,'Precio',1,'0'),(4,4,5,'Total',1,'Cantidad*Precio'),(5,5,6,'Fechas',1,'3'),(6,1,4,'Fecha',3,'0'),(7,2,7,'Artículo',3,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dato`
--

LOCK TABLES `dato` WRITE;
/*!40000 ALTER TABLE `dato` DISABLE KEYS */;
INSERT INTO `dato` VALUES (9,1,1,1,'3'),(10,1,2,2,'Tomates'),(11,1,3,3,'0.23'),(12,1,4,4,'0'),(13,2,1,1,'2'),(14,2,2,2,'Desodorante'),(15,2,3,3,'2.3'),(16,2,4,4,'0'),(17,1,1,6,'2023-04-03'),(19,1,2,7,'1,2'),(20,2,5,5,'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hoja`
--

LOCK TABLES `hoja` WRITE;
/*!40000 ALTER TABLE `hoja` DISABLE KEYS */;
INSERT INTO `hoja` VALUES (1,1,'Artículos','Compras'),(2,1,'Historial','solo un historial'),(3,1,'Fehas','lista de Fecha de la compra');
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programa`
--

LOCK TABLES `programa` WRITE;
/*!40000 ALTER TABLE `programa` DISABLE KEYS */;
INSERT INTO `programa` VALUES (1,'Presupuesto','Para el hogar');
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo`
--

LOCK TABLES `tipo` WRITE;
/*!40000 ALTER TABLE `tipo` DISABLE KEYS */;
INSERT INTO `tipo` VALUES (1,'Texto'),(2,'Número Entero'),(3,'Número Decimal'),(4,'Fecha'),(5,'Cálculo'),(6,'Lista'),(7,'Detalle');
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

-- Dump completed on 2023-04-10 21:52:23
