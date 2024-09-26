-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-09-2024 a las 22:14:37
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `proyecto_inmobiliaria_mvc_guardia_lucero`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id_contrato` int(11) NOT NULL,
  `Id_inquilino` int(11) DEFAULT NULL,
  `Id_inmueble` int(11) DEFAULT NULL,
  `Monto` decimal(10,2) DEFAULT NULL,
  `Fecha_desde` date DEFAULT NULL,
  `Fecha_hasta` date DEFAULT NULL,
  `Monto_Pagar` decimal(10,2) NOT NULL,
  `Estado` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id_contrato`, `Id_inquilino`, `Id_inmueble`, `Monto`, `Fecha_desde`, `Fecha_hasta`, `Monto_Pagar`, `Estado`) VALUES
(1, 6, 6, '50000.00', '2024-09-06', '2024-10-01', '32330.00', '1'),
(3, 1, 1, '6900.00', '2024-09-05', '2024-10-12', '2600.00', '1'),
(4, 4, 2, '17000.00', '2024-09-05', '2024-11-09', '15500.00', '1'),
(10, 1, 1, '5000.00', '2024-11-01', '2024-12-07', '5000.00', '1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id_inmueble` int(11) NOT NULL,
  `Id_propietario` int(11) NOT NULL,
  `Uso` varchar(255) NOT NULL,
  `Tipo` varchar(255) NOT NULL,
  `Ambiente` varchar(255) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Cordenada` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id_inmueble`, `Id_propietario`, `Uso`, `Tipo`, `Ambiente`, `Precio`, `Direccion`, `Cordenada`, `Estado`) VALUES
(1, 8, 'Comercial', 'depósito', '6', '1900.00', 'La punta ,modulo 1 ', '-21344 -21344234', 1),
(2, 11, 'Residencial', 'departamento', '2', '1457.00', 'La Punta,modulo 2', '-38.98800246677288, -61.28767380446236', 1),
(3, 2, 'Residencial', 'casa', '2', '1457.00', 'La Punta,modulo 3', '-21344 -21344234', 1),
(4, 10, 'Residencial', 'casa', '6', '1457.00', 'La Punta,modulo 4', '-21344 -21344234', 1),
(5, 11, 'Residencial', 'depósito', '6', '1457.00', 'La Punta,modulo 5', '-38.98800246677288, -61.28767380446236', 1),
(6, 2, 'Comercial', 'depósito', '7', '349999.00', 'San Luis,', '-38.98800246677288, -61.28767380446236', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id_inquilinos` int(11) NOT NULL,
  `Dni` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefono` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id_inquilinos`, `Dni`, `Apellido`, `Nombre`, `Email`, `Telefono`, `Estado`) VALUES
(1, '5555', 'test', 'test', 'test@test', '266544', 1),
(2, '9239423', 'Kilo', 'Lopexz', 'test2@test', '26645', 1),
(3, '4366634', 'Kilo', 'Jose', 'test@jose', '26645', 1),
(4, '4366634', 'Doe', 'Jon', 'test@example.us', '6019521325', 1),
(5, '78838', 'García Flores', 'Juan Francisco', 'ejemplo@ejemplo.mx', '5553428400', 1),
(6, '89012222', 'Doe', 'Jon', 'test@fr.us', '6019521325', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id_pago` int(11) NOT NULL,
  `Id_contrato` int(11) DEFAULT NULL,
  `Fecha_pago` date DEFAULT NULL,
  `Monto` decimal(10,2) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id_pago`, `Id_contrato`, `Fecha_pago`, `Monto`, `Estado`) VALUES
(1, 1, '2024-09-07', '500.00', 1),
(2, 1, '2024-09-08', '500.00', 1),
(3, 1, '2024-09-10', '7000.00', 1),
(4, 1, '2024-09-14', '9000.00', 1),
(5, 1, '2024-09-18', '570.00', 1),
(6, 1, '2024-09-28', '100.00', 1),
(7, 3, '2024-09-09', '500.00', 0),
(8, 3, '2024-09-12', '5000.00', 0),
(9, 4, '2024-09-14', '1500.00', 1),
(10, 3, '2024-09-11', '700.00', 0),
(11, 3, '2024-09-08', '1300.00', 1),
(12, 3, '2024-09-30', '3000.00', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id_propietarios` int(11) NOT NULL,
  `Dni` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefono` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id_propietarios`, `Dni`, `Apellido`, `Nombre`, `Email`, `Telefono`, `Estado`) VALUES
(1, '53242333', 'Lopez', 'Juan', 'juan@juan', '277891222', 1),
(2, '3912234', 'test100', 'Lopexz', 'kilo@lopez.com', '266999', 1),
(5, '991111', 'Pepe', 'jose', 'test@jose', '324823', 1),
(6, '90122', 'test2', 'Jose', 'test2@test456', '77777', 0),
(7, '4366634', 'test', 'tset', 'test2@test', '3255555', 1),
(8, '45359334', 'Pepe', 'jose', 'pepe@pepe300', '77778', 1),
(9, 'tooltip', 'tooltip', 'tooltip', 'sa@da', '213443', 1),
(10, '82833', 'test', 'test', 'test@test', '90012', 1),
(11, '642333', 'Kilo', 'test2', 'pepe@pepe', '2663992222234', 1),
(12, '8919222', 'test', 'test', 'test30@test', '2663992222', 0),
(13, '90000', 'test', 'test', 'test@jose', '2653244344', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id_usuario` int(11) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Clave` varchar(255) NOT NULL,
  `AvatarUrl` varchar(255) DEFAULT NULL,
  `Rol` int(11) NOT NULL,
  `RolNombre` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id_usuario`, `Nombre`, `Apellido`, `Email`, `Clave`, `AvatarUrl`, `Rol`, `RolNombre`, `Estado`) VALUES
(1, 'Test ', 'test ', 'test@test', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/14d174a4-5a0d-4a64-a9e7-1ec4fe134ece.png', 2, 'Empleado', 1),
(2, 'admin', 'adminA', 'admin@admin', 'PJaQVsoZLo39CH/BQi6SoJ2mu8gD6tQfGAvyButelqg=', '/avatars/default-avatar.png', 1, 'Administrador', 1),
(3, 'jose', 'Pepe', 'test@jose', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/default-avatar.png', 2, 'Empleado', 1),
(13, 'we', 'aweawe', 'admin@admindawd', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/7319eb8a-4011-4283-9b5b-e2d03c75636c.png', 2, 'Empleado', 1),
(14, 'Lopexz', 'Pepe', 'test@test222', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/default-avatar.png', 2, 'Empleado', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id_contrato`),
  ADD KEY `fk_inquilino` (`Id_inquilino`),
  ADD KEY `fk_inmueble` (`Id_inmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id_inmueble`),
  ADD KEY `fk_propietario` (`Id_propietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id_inquilinos`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id_pago`),
  ADD KEY `Id_contrato` (`Id_contrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id_propietarios`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id_usuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id_inquilinos` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id_propietarios` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_inmueble` FOREIGN KEY (`Id_inmueble`) REFERENCES `inmuebles` (`Id_inmueble`),
  ADD CONSTRAINT `fk_inquilino` FOREIGN KEY (`Id_inquilino`) REFERENCES `inquilinos` (`Id_inquilinos`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `fk_propietario` FOREIGN KEY (`Id_propietario`) REFERENCES `propietarios` (`Id_propietarios`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`Id_contrato`) REFERENCES `contrato` (`Id_contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
