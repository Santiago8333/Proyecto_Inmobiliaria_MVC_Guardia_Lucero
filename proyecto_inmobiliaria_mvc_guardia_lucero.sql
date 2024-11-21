-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-11-2024 a las 00:09:14
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
  `Monto_total` decimal(10,2) NOT NULL,
  `Fecha_desde` date DEFAULT NULL,
  `Fecha_hasta` date DEFAULT NULL,
  `FechaTerminacion` date NOT NULL,
  `Monto_Pagar` decimal(10,2) NOT NULL,
  `Meses` int(11) NOT NULL,
  `Estado` tinyint(1) DEFAULT NULL,
  `Contrato_Completado` tinyint(1) NOT NULL,
  `Create_user` varchar(255) NOT NULL,
  `Terminate_user` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id_contrato`, `Id_inquilino`, `Id_inmueble`, `Monto`, `Monto_total`, `Fecha_desde`, `Fecha_hasta`, `FechaTerminacion`, `Monto_Pagar`, `Meses`, `Estado`, `Contrato_Completado`, `Create_user`, `Terminate_user`) VALUES
(15, 3, 3, '350000.00', '2100000.00', '2024-10-01', '2025-01-30', '2024-10-08', '1750000.00', 6, 0, 0, 'admin@admin', 'admin@admin'),
(16, 1, 4, '400000.00', '400000.00', '2024-10-01', '2024-10-30', '2024-10-30', '0.00', 1, 1, 1, 'admin@admin', ''),
(23, 1, 6, '200000.00', '400000.00', '2024-10-01', '2024-11-30', '2024-11-30', '200000.00', 2, 1, 0, 'admin@admin', '');

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
(1, 8, 'Comercial', 'depósito', '3', '300000.00', 'La Punta ,modulo 1, Manzana 108, casa 10', '33°11\'02\"S 66°18\'26\"W', 1),
(2, 11, 'Residencial', 'departamento', '3', '180000.00', 'La Punta, Modulo 7, Manzana 17, casa 2', '33°10\'58\"S 66°19\'11\"W', 1),
(3, 2, 'Residencial', 'casa', '5', '350000.00', 'La Punta, Modulo 3, Manzana 8, casa 18', '33°10\'53\"S 66°19\'00\"W', 1),
(4, 10, 'Residencial', 'casa', '6', '400000.00', 'La Punta,modulo 4, Manzana 15, casa 2', '33°11\'18\"S 66°18\'56\"W', 1),
(5, 11, 'Residencial', 'depósito', '4', '350000.00', 'La Punta,modulo 8, Manzana 22, casa 4', '33°10\'38\"S 66°19\'02\"W', 1),
(6, 2, 'Comercial', 'depósito', '4', '200000.00', 'La Punta, Los Lapachos, Manzana 10, casa 20', '33°09\'52\"S 66°18\'59\"W', 0);

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
(1, '26566324', 'Toledo', 'Martin', 'martintoledo@gmail.com', '2665445425', 1),
(2, '33555221', 'Maure', 'Javier', 'javiermaure@gmail.com', '2664545245', 1),
(3, '43666344', 'Lujan', 'Paulo', 'paulolujan@gmail.com', '26645524521', 1),
(4, '43223441', 'Diaz', 'Manuel', 'manueldiaz@gmail.com', '2664221133', 1),
(5, '31452211', 'García Flores', 'Juan Francisco', 'juangarcia@gmail.com', '2665342840', 1),
(6, '29012222', 'Dominguez', 'Jonathan', 'jonathandominguez@gmail.com', '2664195213', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `multa`
--

CREATE TABLE `multa` (
  `Id_multa` int(11) NOT NULL,
  `Id_contrato` int(11) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `RazonMulta` varchar(500) NOT NULL,
  `Fecha` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `multa`
--

INSERT INTO `multa` (`Id_multa`, `Id_contrato`, `Monto`, `RazonMulta`, `Fecha`) VALUES
(1, 15, '700000.00', 'Multa de 2 Meses,No se Cumplio menos de la mitad del tiempo de la duracion del contrato', '2024-10-08');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id_pago` int(11) NOT NULL,
  `Id_contrato` int(11) DEFAULT NULL,
  `Detalle` varchar(255) NOT NULL,
  `Fecha_pago` date DEFAULT NULL,
  `Monto` decimal(10,2) DEFAULT NULL,
  `Create_user` varchar(255) NOT NULL,
  `Anulado_user` varchar(255) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id_pago`, `Id_contrato`, `Detalle`, `Fecha_pago`, `Monto`, `Create_user`, `Anulado_user`, `Estado`) VALUES
(22, 15, 'Cuota octubre 2024', '2024-10-06', '350000.00', 'admin@admin', 'admin@admin', 0),
(23, 15, 'Cuota octubre 2024', '2024-10-01', '1400000.00', 'admin@admin', 'admin@admin', 0),
(24, 15, 'Cuota octubre 2024', '2024-10-01', '350000.00', 'admin@admin', '', 1),
(25, 15, 'Cuota noviembre 2024', '2024-11-01', '350000.00', 'admin@admin', 'admin@admin', 0),
(26, 16, 'Cuota octubre 2024', '2024-10-01', '400000.00', 'admin@admin', '', 1),
(27, 15, 'Cuota noviembre 2024', '2024-11-01', '350000.00', 'admin@admin', '', 1),
(29, 23, 'Cuota octubre 2024', '2024-10-01', '200000.00', 'admin@admin', '', 1),
(30, 15, 'Cuota diciembre 2024', '2024-12-01', '350000.00', 'admin@admin', '', 1),
(31, 15, 'Cuota diciembre 2024', '2024-12-01', '350000.00', 'admin@admin', 'admin@admin', 0);

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
(1, '32242333', 'Lopez', 'Juan', 'juan@gmail.com', '2664891222', 1),
(2, '28122345', 'Laborda', 'Leonardo', 'leolaborda@gmail.com', '2665214587', 1),
(5, '43421865', 'Cuello', 'Marcos', 'marcoscuello@gmail.com', '2664897452', 1),
(6, '90122', 'test2', 'Jose', 'test2@test456', '77777', 0),
(7, '4366634', 'Figueroa', 'Analia', 'analiafigueroa@gmail.com', '2664512871', 1),
(8, '45333654', 'Camargo', 'Julia', 'julicamargo@gmail.com', '2665873214', 1),
(9, '33564875', 'Miranda', 'Luis', 'luismiranda@gmail.com', '2664212233', 1),
(10, '41258741', 'Molina', 'Maria Jose', 'Mariajose@gmail.com', '2665988985', 1),
(11, '22444562', 'Gauna', 'Dario', 'dariogauna@gmail.com', '2664996635', 1),
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
(1, 'Agustin', 'Toledo', 'agustintoledo@gmail.com', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/14d174a4-5a0d-4a64-a9e7-1ec4fe134ece.png', 2, 'Empleado', 1),
(2, 'admin', 'admin', 'admin@admin', 'FCJBESHoKrpHio8hd8wQ49QdhNSxJHKgkJs9mHAT+6A=', '/avatars/5729b856-a93e-4797-83a9-499edf3fadcd.png', 1, 'Administrador', 1),
(3, 'jose', 'Pepe', 'test@jose', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/default-avatar.png', 2, 'Empleado', 1),
(13, 'Mateo', 'Mateo', 'Mateo@Mateo', 'Hh5PJIf2spPKILQESYS5hbDBI9MT0YpCqENeX/PwcPE=', '/avatars/0a4084ac-16ee-474c-bcac-00c1b007bd8f.png', 2, 'Empleado', 1),
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
-- Indices de la tabla `multa`
--
ALTER TABLE `multa`
  ADD PRIMARY KEY (`Id_multa`),
  ADD KEY `fk_contrato` (`Id_contrato`);

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
  MODIFY `Id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

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
-- AUTO_INCREMENT de la tabla `multa`
--
ALTER TABLE `multa`
  MODIFY `Id_multa` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

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
-- Filtros para la tabla `multa`
--
ALTER TABLE `multa`
  ADD CONSTRAINT `fk_contrato` FOREIGN KEY (`Id_contrato`) REFERENCES `contrato` (`Id_contrato`) ON DELETE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`Id_contrato`) REFERENCES `contrato` (`Id_contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
