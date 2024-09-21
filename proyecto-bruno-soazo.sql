-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-09-2024 a las 20:17:32
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `proyecto-bruno-soazo`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaTerm` date NOT NULL,
  `MontoMensual` double(15,0) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `Anulado` tinyint(1) NOT NULL,
  `IdUsuarioComenzo` int(11) NOT NULL,
  `idUsuarioTermino` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id`, `FechaInicio`, `FechaTerm`, `MontoMensual`, `IdInquilino`, `IdInmueble`, `Anulado`, `IdUsuarioComenzo`, `idUsuarioTermino`) VALUES
(13, '2024-02-02', '2029-02-02', 123456, 1, 1, 1, 10, 10),
(14, '2024-02-02', '2029-02-02', 22222, 11, 2, 1, 10, 10),
(15, '2024-09-21', '2025-09-21', 123456, 1, 1, 0, 10, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(50) NOT NULL,
  `Uso` varchar(20) NOT NULL,
  `Tipo` varchar(20) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Precio` double(11,0) NOT NULL,
  `Latitud` varchar(20) NOT NULL,
  `Longitud` varchar(20) NOT NULL,
  `Disponible` varchar(4) NOT NULL,
  `Propietarioid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Precio`, `Latitud`, `Longitud`, `Disponible`, `Propietarioid`) VALUES
(1, 'MAIPU 900', 'HABITACIONAL', 'LOCAL', 4, 325000, '1234567890', '0987654321', 'SI', 13),
(2, 'PEDERNERA 880', 'HABITACIONAL', 'LOCAL', 3, 125000, '1234567890', '0987654321', 'SI', 8),
(3, 'PEDERNERA 880', 'COMERCIAL', 'LOCAL', 3, 325000, '1234567890', '0987654321', 'SI', 15),
(4, 'SAN MARTIN 88', 'HABITACIONAL', 'DEPOSITO', 2, 325000, '1234567890', '0987654321', 'NO', 13),
(5, 'CHACO 34', 'HABITACIONAL', 'LOCAL', 2, 325000, '1234567890', '0987654321', 'SI', 13),
(6, 'CHACO 34', 'COMERCIAL', 'CASA', 3, 325000, '1234567890', '0987654321', 'SI', 13),
(7, 'CHACO 34', 'HABITACIONAL', 'DEPOSITO', 1, 125000, '1234567890', '0987654321', 'NO', 13);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Dni` varchar(12) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Domicilio` varchar(20) NOT NULL,
  `Ciudad` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Domicilio`, `Ciudad`) VALUES
(1, 'MARCOS', 'ARAGON', '28898345', 'maragon@gmail.com', '2664567890', 'Los Sauces 28', 'S'),
(2, 'RAFAEL', 'LOPEZ', '123456654', 'LPRO@GMAIL.COM', '2663454325', 'LAS HERAS', 'MENDOZA'),
(3, 'MARTA', 'MOYANO', '17876543', 'MMOYANO@GMAIL.COM', '3516789677', 'AGUARIBAY 77', 'CÓRDOBA'),
(5, 'ALBERTO', 'RODRIGUEZ', '46876377', 'RALBERTO@GMAIL.COM', '2664537777', 'TACUARI 80', 'SAN LUIS'),
(6, 'FABRICIO', 'ROMERO', '54872987', 'FROMERO@GMAIL.COM', '2776349987', 'SANTOS ORTIZ 99', 'SAN LUIS'),
(7, 'ALBERTO', 'MENDEZ', '99666123', 'ALBERTM@GMAIL.COM', '344776688', 'LAS HERAS 987', 'SAN LUIS'),
(8, 'PRUDENCIO', 'AUMADA', '18223221', 'JANDRADE@GMAIL.COM', '2664537777', 'FLORIDA 50', 'SAN LUIS'),
(9, 'MARCELO', 'GONZALEZ', '97346213', 'GMARCE@GMAIL.COM', '2554376286', 'AV CERRANA S/N', 'LA PUNTA'),
(10, 'DANIEL', 'SOSA', '44444444', 'DSOSA@GMAIL.COM', '2554376288', 'ITUZAINGO 87', 'SAN LUIS'),
(11, 'MARILINA', 'AGUILERA', '32654876', 'MARILYNGUILERA@GMAIL.COM', '2665432345', 'SAN MARTIN 876', 'SAN LUIS'),
(12, 'RAFAEL', 'RODRIGUEZ', '49288549', 'RPRUDENCIO@GMAIL.COM', '2456378123', 'SAN MARTIN 1234', 'SAN LUIS');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `Numero` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Referencia` varchar(20) NOT NULL,
  `Importe` double NOT NULL,
  `Anulado` varchar(20) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `IdUsuarioComenzo` int(11) NOT NULL,
  `idUsuarioTermino` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `Numero`, `Fecha`, `Referencia`, `Importe`, `Anulado`, `IdContrato`, `IdUsuarioComenzo`, `idUsuarioTermino`) VALUES
(12, 1, '2024-09-21', 'comision', 12333, 'SI', 15, 10, 10);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Dni` varchar(12) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Domicilio` varchar(20) NOT NULL,
  `Ciudad` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Domicilio`, `Ciudad`) VALUES
(1, 'NATALIA', 'SOAZO', '31542891', 'NATALIA.S.LABORAL@GMAIL.COM', '2664344567', 'FLORIDA 50', 'LA PUNTA'),
(3, 'ANAHI', 'CESPEDES', '24567765', 'ncespedes@gmail.com', '2664547689', 'Centenario 879', 'Sa'),
(7, 'CELESTE', 'AUMADA', '49288549', 'CAUMADA@GMAIL.COM', '1324365432', 'SAN MARTIN 80', 'SAN LUIS'),
(8, 'LORENZO', 'BENITEZ', '46876377', 'BLORENZ@GMAIL.COM', '2664536884', 'MITRE 223', 'SAN LUIS'),
(9, 'ALBERTO', 'SOSA', '44444444', 'GHDHHG@GMAIL.COM', '265435366', 'SANTA FE 877', 'SAN LUIS'),
(11, 'ANTONIO', 'CALDERÓN', '2563576536', 'ANTO@IMAIL', '5482314985', 'BELGRANO', 'SAN LUIS'),
(12, 'ALEJO', 'PEREZ', '2346519835', 'ALEJO@MAIL', '2664329754', 'SERRANA 76', 'SAN LUIS'),
(13, 'LISANDRO', 'AGUILERA', '4563789210', 'LIA@MAIL', '2664873452', 'SAN MARTÍN', 'SAN LUIS'),
(14, 'ABEL', 'CASTILLO', '1234554321', 'ABELITO@MAIL', '2664231768', 'MENDOZA', 'SAN LUIS'),
(15, 'TEODORO', 'BLANCO', '266754389', 'TEO@MAIL', '5367823145', 'CORDOBA', 'SAN LUIS'),
(16, 'VICTORIA', 'ROSARIO', '4256341724', 'VICKI@MAIL', '2456378123', 'MISIONES 800', 'SAN LUIS'),
(17, 'PRUDENCIO', 'ANDRADE', '4256341724', 'JANDRADE@GMAIL.COM', '2554346578', '90', 'SAN LUIS');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `Numero` int(1) NOT NULL,
  `Rol` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`Numero`, `Rol`) VALUES
(1, 'EMPLEADO'),
(2, 'ADMINISTRADOR');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmuebles`
--

CREATE TABLE `tipoinmuebles` (
  `Tipo` varchar(20) NOT NULL,
  `Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmuebles`
--

INSERT INTO `tipoinmuebles` (`Tipo`, `Id`) VALUES
('LOCAL', 1),
('DEPOSITO', 2),
('CASA', 3),
('DEPARTAMENTO', 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Correo` varchar(30) NOT NULL,
  `Clave` varchar(1500) NOT NULL,
  `Rol` int(2) NOT NULL,
  `AvatarURL` varchar(30) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `Correo`, `Clave`, `Rol`, `AvatarURL`) VALUES
(7, 'LAURA', 'SUAREZ', 'lauraperez12@gmail.com', 'Mhc3bPUHnAoj11+xusvyNM8BJsg3P3qeevTViybLJ/Q=', 1, '/ImgSubidas\\anonimo.jpg'),
(9, 'GIULIETTA', 'BRUNO', 'jes@gmail.com', 'orG0/zZ0z0h2Zv102mCFIWuW7ShFJcquhvt2i01OeuY=', 2, '/ImgSubidas\\anonimo.jpg'),
(10, 'GIULIETTA', 'BRUNO', 'giulietta133@gmail.com', 'Mhc3bPUHnAoj11+xusvyNM8BJsg3P3qeevTViybLJ/Q=', 2, '/ImgSubidas\\anonimo.jpg'),
(11, 'CARLOS', 'PEREZ', 'we332@gmial.com', 'Mhc3bPUHnAoj11+xusvyNM8BJsg3P3qeevTViybLJ/Q=', 1, '/ImgSubidas\\anonimo.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdInquilino` (`IdInquilino`),
  ADD KEY `IdInmueble` (`IdInmueble`),
  ADD KEY `idUsuarioTermino` (`idUsuarioTermino`),
  ADD KEY `IdUsuarioComenzo` (`IdUsuarioComenzo`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `Propietarioid` (`Propietarioid`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdContrato` (`IdContrato`),
  ADD KEY `IdUsuarioComenzo` (`IdUsuarioComenzo`),
  ADD KEY `idUsuarioTermino` (`idUsuarioTermino`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Numero`);

--
-- Indices de la tabla `tipoinmuebles`
--
ALTER TABLE `tipoinmuebles`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `roles`
--
ALTER TABLE `roles`
  MODIFY `Numero` int(1) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `tipoinmuebles`
--
ALTER TABLE `tipoinmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilinos` (`Id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`IdInmueble`) REFERENCES `inmuebles` (`Id`),
  ADD CONSTRAINT `contratos_ibfk_3` FOREIGN KEY (`idUsuarioTermino`) REFERENCES `usuarios` (`Id`),
  ADD CONSTRAINT `contratos_ibfk_4` FOREIGN KEY (`IdUsuarioComenzo`) REFERENCES `usuarios` (`Id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`Propietarioid`) REFERENCES `propietarios` (`Id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`Id`),
  ADD CONSTRAINT `pagos_ibfk_2` FOREIGN KEY (`IdUsuarioComenzo`) REFERENCES `usuarios` (`Id`),
  ADD CONSTRAINT `pagos_ibfk_3` FOREIGN KEY (`idUsuarioTermino`) REFERENCES `usuarios` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
