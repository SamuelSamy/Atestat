-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 07, 2021 at 08:09 PM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `atestat`
--

-- --------------------------------------------------------

--
-- Table structure for table `anunturi`
--

CREATE TABLE `anunturi` (
  `id` int(11) NOT NULL COMMENT 'Cheie primara\r\nAutoincrement',
  `idUtilizator` int(11) NOT NULL COMMENT 'Id-ul utilizatorului\r\ncare a publicat anuntul',
  `titlu` date NOT NULL COMMENT 'Titlul anuntului',
  `pret` int(11) NOT NULL COMMENT 'Pretul anuntului',
  `descrierere` int(11) NOT NULL COMMENT 'Descrierea anuntului',
  `dataIncarcare` date NOT NULL COMMENT 'Data la care a fost incarcat anuntul',
  `vizualizari` int(11) NOT NULL COMMENT 'Numarul de vizualizari care au accesat anuntul',
  `nrImagini` int(11) NOT NULL COMMENT 'Numarul de imagini',
  `etichete` text NOT NULL COMMENT 'Etichetele anuntului',
  `idCategorie` int(11) NOT NULL COMMENT 'Categoria din care face parte anuntul'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `categorii`
--

CREATE TABLE `categorii` (
  `id` int(11) NOT NULL,
  `nume` text NOT NULL COMMENT 'Numele categoriei'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `pedepse`
--

CREATE TABLE `pedepse` (
  `id` int(11) NOT NULL,
  `idUtilizator` int(11) NOT NULL COMMENT 'Id-ul utilizatorului',
  `pedeapsa` text NOT NULL COMMENT 'Suspedenare cont / Revocarea dreptului de a publica anunturi',
  `motiv` text NOT NULL COMMENT 'Motivul pedepsei',
  `data` date NOT NULL COMMENT 'Data la care utilizatorul a primit pedeapsa',
  `durata` int(11) NOT NULL COMMENT 'Durata in zile a pedepsei'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `reclamatii`
--

CREATE TABLE `reclamatii` (
  `id` int(11) NOT NULL,
  `idReclamat` int(11) NOT NULL COMMENT 'Id-ul persoanei care a facut reclamatia',
  `idReclamant` int(11) NOT NULL COMMENT 'Id-ul persoanei reclamate',
  `data` date NOT NULL COMMENT 'Data la care utilizatorul a fost reclamat',
  `descriere` text NOT NULL COMMENT 'Motivul pentru care utilizatorul a fost reclamat',
  `tipReclamare` text NOT NULL COMMENT 'Utilizator / Anunt',
  `verificat` tinyint(1) NOT NULL COMMENT 'True daca reclamatia a fost rezolvata de catre un admin, False in caz contrar'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `utilizatori`
--

CREATE TABLE `utilizatori` (
  `id` int(11) NOT NULL COMMENT 'Cheie Primara\r\nAutoincrement',
  `nume` text NOT NULL COMMENT 'Numele utilizatorului',
  `mail` text NOT NULL COMMENT 'Adresa de mail a utilizatorului',
  `dataInregistrare` date NOT NULL COMMENT 'Data la care a fost inregistrat utilizatorul',
  `nrTelefon` text NOT NULL COMMENT 'Numarul de telefon a utilizatorului',
  `parola` text NOT NULL COMMENT 'Parola utilizatorului',
  `tipCont` text NOT NULL COMMENT 'Normal / Verificat / Administrator / Proprietar'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `anunturi`
--
ALTER TABLE `anunturi`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `categorii`
--
ALTER TABLE `categorii`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `pedepse`
--
ALTER TABLE `pedepse`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `reclamatii`
--
ALTER TABLE `reclamatii`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `utilizatori`
--
ALTER TABLE `utilizatori`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `anunturi`
--
ALTER TABLE `anunturi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Cheie primara\r\nAutoincrement';

--
-- AUTO_INCREMENT for table `categorii`
--
ALTER TABLE `categorii`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `pedepse`
--
ALTER TABLE `pedepse`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reclamatii`
--
ALTER TABLE `reclamatii`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `utilizatori`
--
ALTER TABLE `utilizatori`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Cheie Primara\r\nAutoincrement';
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
