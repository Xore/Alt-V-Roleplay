/*
 Navicat Premium Data Transfer

 Source Server         : Local
 Source Server Type    : MySQL
 Source Server Version : 80028
 Source Host           : localhost:3306
 Source Schema         : altv

 Target Server Type    : MySQL
 Target Server Version : 80028
 File Encoding         : 65001

 Date: 17/03/2022 14:29:48
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'N/A',
  `email` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `socialid` bigint(0) UNSIGNED NOT NULL,
  `password` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hwid` bigint(0) UNSIGNED NOT NULL DEFAULT 0,
  `online` int(0) NOT NULL DEFAULT 0,
  `whitelisted` tinyint(1) NOT NULL,
  `ban` tinyint(1) NOT NULL,
  `banReason` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Nicht Gebannt',
  `adminlevel` int(0) NOT NULL DEFAULT 0,
  `ip` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  `userava` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'themes/uploads/Main_NoG_Logo.jpg',
  `whitelistSended` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for accounts_characters
-- ----------------------------
DROP TABLE IF EXISTS `accounts_characters`;
CREATE TABLE `accounts_characters`  (
  `charId` int(0) NOT NULL AUTO_INCREMENT,
  `accountId` int(0) NOT NULL,
  `charname` varchar(35) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `death` tinyint(1) NOT NULL,
  `accState` int(0) NOT NULL DEFAULT 0,
  `firstjoin` tinyint(1) NOT NULL,
  `firstspawnplace` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `firstJoinTimestamp` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `gender` tinyint(1) NOT NULL COMMENT '0 = male, 1 = female',
  `birthdate` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `birthplace` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `health` int(0) NOT NULL DEFAULT 100,
  `armor` int(0) NOT NULL DEFAULT 0,
  `hunger` int(0) NOT NULL DEFAULT 100,
  `thirst` int(0) NOT NULL DEFAULT 100,
  `address` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Boulevard Del Perro 2a',
  `phonenumber` int(0) NOT NULL,
  `isCrime` tinyint(1) NOT NULL DEFAULT 0,
  `paydayTime` int(0) NOT NULL,
  `weapon_primary` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `weapon_primary_ammo` int(0) NOT NULL DEFAULT 0,
  `weapon_secondary` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `weapon_secondary_ammo` int(0) NOT NULL DEFAULT 0,
  `weapon_secondary2` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `weapon_secondary2_ammo` int(0) NOT NULL DEFAULT 0,
  `weapon_fist` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `weapon_fist_ammo` int(0) NOT NULL DEFAULT 0,
  `isUnconscious` tinyint(1) NOT NULL DEFAULT 0,
  `unconsciousTime` int(0) NOT NULL DEFAULT 0,
  `isFastFarm` tinyint(1) NOT NULL DEFAULT 0,
  `fastFarmTime` int(0) NOT NULL DEFAULT 0,
  `lastLogin` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `isPhoneEquipped` tinyint(1) NOT NULL DEFAULT 0,
  `playtimeHours` int(0) NOT NULL DEFAULT 0,
  `isInJail` tinyint(1) NOT NULL DEFAULT 0,
  `jailTime` int(0) NOT NULL DEFAULT 0,
  `wallpaper` int(0) NULL DEFAULT 1,
  `pedName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'none',
  `isAnimalPed` int(0) NOT NULL DEFAULT 0,
  `alcLvl` float NOT NULL DEFAULT 0,
  `drug` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Clean',
  PRIMARY KEY (`charId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for characters_bank
-- ----------------------------
DROP TABLE IF EXISTS `characters_bank`;
CREATE TABLE `characters_bank`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charid` int(0) NOT NULL,
  `accountnumber` int(0) NOT NULL,
  `money` int(0) NOT NULL,
  `pin` int(0) NOT NULL,
  `mainaccount` tinyint(1) NOT NULL DEFAULT 0,
  `closed` tinyint(1) NOT NULL DEFAULT 0 COMMENT '1 = gesperrt, 0 = entsperrt',
  `pinTrys` int(0) NOT NULL DEFAULT 0 COMMENT 'wie oft ein PIN falsch eingegeben wurde',
  `createZone` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_inventory
-- ----------------------------
DROP TABLE IF EXISTS `characters_inventory`;
CREATE TABLE `characters_inventory`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charid` int(0) NOT NULL,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `itemAmount` int(0) NOT NULL,
  `itemLocation` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_lastpos
-- ----------------------------
DROP TABLE IF EXISTS `characters_lastpos`;
CREATE TABLE `characters_lastpos`  (
  `charid` int(0) NOT NULL,
  `lastPosX` float NOT NULL,
  `lastPosY` float NOT NULL,
  `lastPosZ` float NOT NULL,
  `lastDimension` int(0) NOT NULL,
  PRIMARY KEY (`charid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_licenses
-- ----------------------------
DROP TABLE IF EXISTS `characters_licenses`;
CREATE TABLE `characters_licenses`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `pkw` tinyint(1) NOT NULL DEFAULT 0,
  `lkw` tinyint(1) NOT NULL DEFAULT 0,
  `bike` tinyint(1) NOT NULL DEFAULT 0,
  `boat` tinyint(1) NOT NULL DEFAULT 0,
  `fly` tinyint(1) NOT NULL DEFAULT 0,
  `helicopter` tinyint(1) NOT NULL DEFAULT 0,
  `passengertransport` tinyint(1) NOT NULL DEFAULT 0,
  `weaponlicense` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_minijobs
-- ----------------------------
DROP TABLE IF EXISTS `characters_minijobs`;
CREATE TABLE `characters_minijobs`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `jobName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `exp` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_ownedclothes
-- ----------------------------
DROP TABLE IF EXISTS `characters_ownedclothes`;
CREATE TABLE `characters_ownedclothes`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `clothId` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_permissions
-- ----------------------------
DROP TABLE IF EXISTS `characters_permissions`;
CREATE TABLE `characters_permissions`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `permissionName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_phone_chatmessages
-- ----------------------------
DROP TABLE IF EXISTS `characters_phone_chatmessages`;
CREATE TABLE `characters_phone_chatmessages`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `chatId` int(0) NOT NULL,
  `fromNumber` int(0) NOT NULL,
  `toNumber` int(0) NOT NULL,
  `unix` int(0) NOT NULL,
  `message` varchar(2560) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_phone_chats
-- ----------------------------
DROP TABLE IF EXISTS `characters_phone_chats`;
CREATE TABLE `characters_phone_chats`  (
  `chatId` int(0) NOT NULL AUTO_INCREMENT,
  `phoneNumber` int(0) NOT NULL,
  `anotherNumber` int(0) NOT NULL,
  PRIMARY KEY (`chatId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_phone_contacts
-- ----------------------------
DROP TABLE IF EXISTS `characters_phone_contacts`;
CREATE TABLE `characters_phone_contacts`  (
  `contactId` int(0) NOT NULL AUTO_INCREMENT,
  `phoneNumber` int(0) NOT NULL,
  `contactName` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `contactNumber` int(0) NOT NULL,
  PRIMARY KEY (`contactId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_skin
-- ----------------------------
DROP TABLE IF EXISTS `characters_skin`;
CREATE TABLE `characters_skin`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `facefeatures` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `headblendsdata` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `headoverlays` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `clothesTop` int(0) NULL DEFAULT NULL,
  `clothesTorso` int(0) NULL DEFAULT NULL,
  `clothesLeg` int(0) NULL DEFAULT NULL,
  `clothesFeet` int(0) NULL DEFAULT NULL,
  `clothesHat` int(0) NULL DEFAULT NULL,
  `clothesGlass` int(0) NULL DEFAULT NULL,
  `clothesEarring` int(0) NULL DEFAULT NULL,
  `clothesNecklace` int(0) NULL DEFAULT NULL,
  `clothesMask` int(0) NULL DEFAULT NULL,
  `clothesArmor` int(0) NULL DEFAULT NULL,
  `clothesUndershirt` int(0) NULL DEFAULT NULL,
  `clothesBracelet` int(0) NULL DEFAULT NULL,
  `clothesWatch` int(0) NULL DEFAULT NULL,
  `clothesBag` int(0) NULL DEFAULT NULL,
  `clothesDecal` int(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for characters_tablet_apps
-- ----------------------------
DROP TABLE IF EXISTS `characters_tablet_apps`;
CREATE TABLE `characters_tablet_apps`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `weather` tinyint(1) NOT NULL DEFAULT 0,
  `news` tinyint(1) NOT NULL DEFAULT 0,
  `banking` tinyint(1) NOT NULL DEFAULT 0,
  `lifeinvader` tinyint(1) NOT NULL DEFAULT 0,
  `vehicles` tinyint(1) NOT NULL DEFAULT 0,
  `events` tinyint(1) NOT NULL DEFAULT 0,
  `company` tinyint(1) NOT NULL DEFAULT 0,
  `buyapp` tinyint(1) NOT NULL DEFAULT 0,
  `notices` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_tablet_tutorial
-- ----------------------------
DROP TABLE IF EXISTS `characters_tablet_tutorial`;
CREATE TABLE `characters_tablet_tutorial`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `openTablet` tinyint(1) NOT NULL DEFAULT 0,
  `openInventory` tinyint(1) NOT NULL DEFAULT 0,
  `createBankAccount` tinyint(1) NOT NULL DEFAULT 0,
  `buyVehicle` tinyint(1) NOT NULL DEFAULT 0,
  `useGarage` tinyint(1) NOT NULL DEFAULT 0,
  `acceptJob` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_tattoos
-- ----------------------------
DROP TABLE IF EXISTS `characters_tattoos`;
CREATE TABLE `characters_tattoos`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `tattooId` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for characters_wanteds
-- ----------------------------
DROP TABLE IF EXISTS `characters_wanteds`;
CREATE TABLE `characters_wanteds`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `wantedId` int(0) NOT NULL,
  `givenString` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for config
-- ----------------------------
DROP TABLE IF EXISTS `config`;
CREATE TABLE `config`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `site_dl_section` int(0) NOT NULL,
  `site_rage_section` int(0) NOT NULL,
  `site_altv_section` int(0) NOT NULL,
  `site_fivem_section` int(0) NOT NULL,
  `site_online` int(0) NOT NULL,
  `site_name` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for news_lang
-- ----------------------------
DROP TABLE IF EXISTS `news_lang`;
CREATE TABLE `news_lang`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `title` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `title_de` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `content` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `content_de` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_all_vehicles
-- ----------------------------
DROP TABLE IF EXISTS `server_all_vehicles`;
CREATE TABLE `server_all_vehicles`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `category` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `manufactor` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hash` bigint(0) NOT NULL,
  `price` int(0) NOT NULL,
  `trunkcapacity` int(0) NOT NULL,
  `maxfuel` int(0) NOT NULL,
  `fueltype` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `seats` int(0) NOT NULL,
  `tax` int(0) NOT NULL DEFAULT 0 COMMENT 'Fahrzeugsteuer',
  `vehClass` int(0) NOT NULL COMMENT '0 - Auto | 1- Boot | 2 - Flugzeug | 3 - Helikopter',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_animations
-- ----------------------------
DROP TABLE IF EXISTS `server_animations`;
CREATE TABLE `server_animations`  (
  `animId` int(0) NOT NULL AUTO_INCREMENT,
  `displayName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `animDict` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `animName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `animFlag` int(0) NOT NULL,
  `duration` int(0) NOT NULL DEFAULT -1,
  `category` int(0) NOT NULL,
  PRIMARY KEY (`animId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_atm
-- ----------------------------
DROP TABLE IF EXISTS `server_atm`;
CREATE TABLE `server_atm`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `zoneName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `isrobbed` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_bank_paper
-- ----------------------------
DROP TABLE IF EXISTS `server_bank_paper`;
CREATE TABLE `server_bank_paper`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `accountNumber` int(0) NOT NULL,
  `Date` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Time` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Type` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ToOrFrom` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `TransactionMessage` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `moneyAmount` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `zoneName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_banks
-- ----------------------------
DROP TABLE IF EXISTS `server_banks`;
CREATE TABLE `server_banks`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `zoneName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_barbers
-- ----------------------------
DROP TABLE IF EXISTS `server_barbers`;
CREATE TABLE `server_barbers`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `pedModel` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `pedX` float NOT NULL,
  `pedY` float NOT NULL,
  `pedZ` float NOT NULL,
  `pedRot` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_blips
-- ----------------------------
DROP TABLE IF EXISTS `server_blips`;
CREATE TABLE `server_blips`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `color` int(0) NOT NULL,
  `scale` float NOT NULL,
  `shortRange` tinyint(1) NOT NULL,
  `sprite` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_blitzer
-- ----------------------------
DROP TABLE IF EXISTS `server_blitzer`;
CREATE TABLE `server_blitzer`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `objectPos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `objectRotation` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `colshapePos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `colshapeRadius` float NOT NULL,
  `speedLimit` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_calls
-- ----------------------------
DROP TABLE IF EXISTS `server_calls`;
CREATE TABLE `server_calls`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `number` int(0) NOT NULL,
  `result` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `timestamp` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_car_rentals
-- ----------------------------
DROP TABLE IF EXISTS `server_car_rentals`;
CREATE TABLE `server_car_rentals`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pedPos` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pedRot` float NOT NULL,
  `parkOutPos` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `parkOutRot` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_car_rentals_vehicles
-- ----------------------------
DROP TABLE IF EXISTS `server_car_rentals_vehicles`;
CREATE TABLE `server_car_rentals_vehicles`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `carRentalId` int(0) NOT NULL,
  `hash` bigint(0) UNSIGNED NOT NULL,
  `pricePerDay` int(0) NOT NULL,
  `vehPos` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `vehRot` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_clothesshops
-- ----------------------------
DROP TABLE IF EXISTS `server_clothesshops`;
CREATE TABLE `server_clothesshops`  (
  `id` int(0) NOT NULL,
  `name` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `pedX` float NOT NULL,
  `pedY` float NOT NULL,
  `pedZ` float NOT NULL,
  `pedRot` float NOT NULL,
  `pedModel` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_clothesshops_items
-- ----------------------------
DROP TABLE IF EXISTS `server_clothesshops_items`;
CREATE TABLE `server_clothesshops_items`  (
  `id` int(0) NOT NULL,
  `componentId` int(0) NOT NULL,
  `drawableId` int(0) NOT NULL,
  `textureId` int(0) NOT NULL,
  `gender` int(0) NOT NULL,
  `isProp` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_crafting_recipes
-- ----------------------------
DROP TABLE IF EXISTS `server_crafting_recipes`;
CREATE TABLE `server_crafting_recipes`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `craftingStations` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `neededItems` varchar(512) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `endItem` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `duration` int(0) NOT NULL DEFAULT 2500,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_crafting_stations
-- ----------------------------
DROP TABLE IF EXISTS `server_crafting_stations`;
CREATE TABLE `server_crafting_stations`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `pos` varchar(128) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '{ \"x\": 0.0, \"y\": 0.0, \"z\": 0.0 }',
  `heading` float NOT NULL DEFAULT 0,
  `pedModel` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `isBlipVisible` tinyint(1) NOT NULL DEFAULT 1,
  `comment` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_doors
-- ----------------------------
DROP TABLE IF EXISTS `server_doors`;
CREATE TABLE `server_doors`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `mainDoor` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `secondDoor` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `state` tinyint(1) NOT NULL DEFAULT 1 COMMENT '1 = abgeschlossen',
  `doorKey` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Generalschluessel',
  `doorKey2` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `type` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Door',
  `lockPosX` float NOT NULL,
  `lockPosY` float NOT NULL,
  `lockPosZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_dropped_items
-- ----------------------------
DROP TABLE IF EXISTS `server_dropped_items`;
CREATE TABLE `server_dropped_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `itemAmount` int(0) NOT NULL,
  `pos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `droppedTimestamp` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `dimension` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_faction_dispatches
-- ----------------------------
DROP TABLE IF EXISTS `server_faction_dispatches`;
CREATE TABLE `server_faction_dispatches`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `factionId` int(0) NOT NULL,
  `senderCharId` int(0) NOT NULL,
  `message` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `date` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `destination` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `altname` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `Time` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_faction_members
-- ----------------------------
DROP TABLE IF EXISTS `server_faction_members`;
CREATE TABLE `server_faction_members`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `factionId` int(0) NOT NULL,
  `rank` int(0) NOT NULL DEFAULT 1,
  `servicenumber` int(0) NOT NULL DEFAULT 0,
  `isDuty` tinyint(1) NOT NULL DEFAULT 0,
  `lastChange` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_faction_positions
-- ----------------------------
DROP TABLE IF EXISTS `server_faction_positions`;
CREATE TABLE `server_faction_positions`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `factionId` int(0) NOT NULL,
  `posType` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'storage | duty | manage',
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotation` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_faction_ranks
-- ----------------------------
DROP TABLE IF EXISTS `server_faction_ranks`;
CREATE TABLE `server_faction_ranks`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `factionId` int(0) NOT NULL,
  `rankId` int(0) NOT NULL,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `paycheck` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_faction_storage_items
-- ----------------------------
DROP TABLE IF EXISTS `server_faction_storage_items`;
CREATE TABLE `server_faction_storage_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `factionId` int(0) NOT NULL,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_factions
-- ----------------------------
DROP TABLE IF EXISTS `server_factions`;
CREATE TABLE `server_factions`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `factionName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `factionShort` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `factionMoney` int(0) NOT NULL DEFAULT 0,
  `phoneNumber` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_farming_producer
-- ----------------------------
DROP TABLE IF EXISTS `server_farming_producer`;
CREATE TABLE `server_farming_producer`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `pedRotation` float NOT NULL,
  `pedModel` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `neededItem` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `producedItem` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `range` float NOT NULL,
  `duration` int(0) NOT NULL,
  `neededItemAmount` int(0) NOT NULL,
  `producedItemAmount` int(0) NOT NULL,
  `blipName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `isBlipVisible` tinyint(1) NOT NULL,
  `neededItemTWO` varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `neededItemTHREE` varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `neededItemTWOAmount` int(0) NULL DEFAULT NULL,
  `neededItemTHREEAmount` int(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_farming_spots
-- ----------------------------
DROP TABLE IF EXISTS `server_farming_spots`;
CREATE TABLE `server_farming_spots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `itemName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `animation` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `neededItemToFarm` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `itemMinAmount` int(0) NOT NULL,
  `itemMaxAmount` int(0) NOT NULL,
  `markerColorR` int(0) NOT NULL,
  `markerColorG` int(0) NOT NULL,
  `markerColorB` int(0) NOT NULL,
  `blipColor` int(0) NOT NULL DEFAULT 1,
  `range` float NOT NULL,
  `duration` int(0) NOT NULL,
  `isBlipVisible` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_fuel_spots
-- ----------------------------
DROP TABLE IF EXISTS `server_fuel_spots`;
CREATE TABLE `server_fuel_spots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `fuelStationId` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_fuel_stations
-- ----------------------------
DROP TABLE IF EXISTS `server_fuel_stations`;
CREATE TABLE `server_fuel_stations`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `owner` int(0) NOT NULL DEFAULT 0,
  `availableFuel` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `availableLiters` int(0) NOT NULL DEFAULT 0,
  `bank` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_gang_members
-- ----------------------------
DROP TABLE IF EXISTS `server_gang_members`;
CREATE TABLE `server_gang_members`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `gangId` int(0) NOT NULL,
  `rank` int(0) NOT NULL DEFAULT 1,
  `lastChange` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_gang_positions
-- ----------------------------
DROP TABLE IF EXISTS `server_gang_positions`;
CREATE TABLE `server_gang_positions`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `gangId` int(0) NOT NULL,
  `posType` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'storage | duty | manage',
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotation` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_gang_ranks
-- ----------------------------
DROP TABLE IF EXISTS `server_gang_ranks`;
CREATE TABLE `server_gang_ranks`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `gangId` int(0) NOT NULL,
  `rankId` int(0) NOT NULL,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `paycheck` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_gang_storage_items
-- ----------------------------
DROP TABLE IF EXISTS `server_gang_storage_items`;
CREATE TABLE `server_gang_storage_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `gangId` int(0) NOT NULL,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_gangs
-- ----------------------------
DROP TABLE IF EXISTS `server_gangs`;
CREATE TABLE `server_gangs`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `gangName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `gangShort` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_garage_slots
-- ----------------------------
DROP TABLE IF EXISTS `server_garage_slots`;
CREATE TABLE `server_garage_slots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `garageId` int(0) NOT NULL,
  `parkid` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotX` float NOT NULL,
  `rotY` float NOT NULL,
  `rotZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_garages
-- ----------------------------
DROP TABLE IF EXISTS `server_garages`;
CREATE TABLE `server_garages`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotation` float NOT NULL,
  `type` int(0) NOT NULL DEFAULT 0 COMMENT '0 Auto | 1 Boot | 2 Flugzeug | 3 Heli',
  `isBlipVisible` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_hotels
-- ----------------------------
DROP TABLE IF EXISTS `server_hotels`;
CREATE TABLE `server_hotels`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_hotels_apartments
-- ----------------------------
DROP TABLE IF EXISTS `server_hotels_apartments`;
CREATE TABLE `server_hotels_apartments`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `hotelId` int(0) NOT NULL,
  `interiorId` int(0) NOT NULL,
  `ownerId` int(0) NOT NULL DEFAULT 0,
  `rentPrice` int(0) NOT NULL DEFAULT 0,
  `maxRentHours` int(0) NOT NULL DEFAULT 72,
  `lastRent` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_hotels_storage
-- ----------------------------
DROP TABLE IF EXISTS `server_hotels_storage`;
CREATE TABLE `server_hotels_storage`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `apartmentId` int(0) NOT NULL,
  `itemName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_houses
-- ----------------------------
DROP TABLE IF EXISTS `server_houses`;
CREATE TABLE `server_houses`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `interiorId` int(0) NOT NULL,
  `ownerId` int(0) NOT NULL,
  `street` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `price` int(0) NOT NULL DEFAULT 0,
  `maxRenters` int(0) NOT NULL,
  `rentPrice` int(0) NOT NULL DEFAULT 0,
  `isRentable` tinyint(1) NOT NULL DEFAULT 0,
  `hasStorage` tinyint(1) NOT NULL DEFAULT 0,
  `hasAlarm` tinyint(1) NOT NULL DEFAULT 0,
  `hasBank` tinyint(1) NOT NULL DEFAULT 0,
  `entranceX` float NOT NULL,
  `entranceY` float NOT NULL,
  `entranceZ` float NOT NULL,
  `money` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_houses_interior
-- ----------------------------
DROP TABLE IF EXISTS `server_houses_interior`;
CREATE TABLE `server_houses_interior`  (
  `interiorId` int(0) NOT NULL AUTO_INCREMENT,
  `exitX` float NOT NULL,
  `exitY` float NOT NULL,
  `exitZ` float NOT NULL,
  `storageX` float NOT NULL,
  `storageY` float NOT NULL,
  `storageZ` float NOT NULL,
  `storageLimit` float NOT NULL,
  `manageX` float NOT NULL DEFAULT 0,
  `manageY` float NOT NULL DEFAULT 0,
  `manageZ` float NOT NULL DEFAULT 0,
  PRIMARY KEY (`interiorId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_houses_renter
-- ----------------------------
DROP TABLE IF EXISTS `server_houses_renter`;
CREATE TABLE `server_houses_renter`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `houseId` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_houses_storage
-- ----------------------------
DROP TABLE IF EXISTS `server_houses_storage`;
CREATE TABLE `server_houses_storage`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `houseId` int(0) NOT NULL,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_items
-- ----------------------------
DROP TABLE IF EXISTS `server_items`;
CREATE TABLE `server_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `itemName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `itemType` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `itemDescription` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `itemWeight` float NOT NULL,
  `isItemDesire` tinyint(1) NOT NULL DEFAULT 0,
  `itemDesireFood` int(0) NOT NULL DEFAULT 0,
  `itemDesireDrink` int(0) NOT NULL DEFAULT 0,
  `hasItemAnimation` tinyint(1) NOT NULL DEFAULT 0,
  `itemAnimationName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `isItemDroppable` tinyint(1) NOT NULL DEFAULT 1,
  `isItemUseable` tinyint(1) NOT NULL DEFAULT 1,
  `isItemGiveable` tinyint(0) NOT NULL DEFAULT 1,
  `itemPicSRC` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_jobs
-- ----------------------------
DROP TABLE IF EXISTS `server_jobs`;
CREATE TABLE `server_jobs`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `jobName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `jobPaycheck` int(0) NOT NULL DEFAULT 0,
  `jobNeededHours` int(0) NOT NULL DEFAULT 0,
  `jobPic` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_licenses
-- ----------------------------
DROP TABLE IF EXISTS `server_licenses`;
CREATE TABLE `server_licenses`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `licCut` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `licName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `licPrice` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_markers
-- ----------------------------
DROP TABLE IF EXISTS `server_markers`;
CREATE TABLE `server_markers`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `type` int(0) NOT NULL DEFAULT 1,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `scaleX` float NOT NULL,
  `scaleY` float NOT NULL,
  `scaleZ` float NOT NULL,
  `red` int(0) NOT NULL,
  `green` int(0) NOT NULL,
  `blue` int(0) NOT NULL,
  `alpha` int(0) NOT NULL DEFAULT 255,
  `bobUpAndDown` tinyint(1) NOT NULL DEFAULT 0,
  `kommentar` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT ' ',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = COMPACT;

-- ----------------------------
-- Table structure for server_minijob_busdriver_routes
-- ----------------------------
DROP TABLE IF EXISTS `server_minijob_busdriver_routes`;
CREATE TABLE `server_minijob_busdriver_routes`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `routeId` int(0) NOT NULL,
  `routeName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hash` bigint(0) UNSIGNED NOT NULL,
  `neededExp` int(0) NOT NULL,
  `givenExp` int(0) NOT NULL,
  `paycheck` int(0) NOT NULL,
  `neededTime` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_minijob_busdriver_spots
-- ----------------------------
DROP TABLE IF EXISTS `server_minijob_busdriver_spots`;
CREATE TABLE `server_minijob_busdriver_spots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `routeId` int(0) NOT NULL,
  `spotId` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_minijob_garbage_spots
-- ----------------------------
DROP TABLE IF EXISTS `server_minijob_garbage_spots`;
CREATE TABLE `server_minijob_garbage_spots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `routeId` int(0) NOT NULL,
  `spotId` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_objects
-- ----------------------------
DROP TABLE IF EXISTS `server_objects`;
CREATE TABLE `server_objects`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `itemHash` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `pos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `rotX` float NOT NULL,
  `rotY` float NOT NULL,
  `rotZ` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_peds
-- ----------------------------
DROP TABLE IF EXISTS `server_peds`;
CREATE TABLE `server_peds`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `model` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotation` float NOT NULL,
  `kommentar` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_shops
-- ----------------------------
DROP TABLE IF EXISTS `server_shops`;
CREATE TABLE `server_shops`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `owner` int(0) NOT NULL DEFAULT 0,
  `bank` int(0) NOT NULL DEFAULT 0,
  `price` int(0) NOT NULL DEFAULT 0,
  `pedPos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `shopPos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `managePos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `pedModel` varchar(25) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL DEFAULT 'mp_m_shopkeep_01',
  `pedRot` float NOT NULL DEFAULT 0,
  `blipColor` int(0) NOT NULL DEFAULT 2,
  `blipSprite` int(0) NOT NULL DEFAULT 52,
  `type` int(0) NOT NULL DEFAULT 0,
  `faction` int(0) NOT NULL DEFAULT 0,
  `neededLicense` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  `isOnlySelling` tinyint(1) NOT NULL DEFAULT 0,
  `isBlipVisible` tinyint(1) NOT NULL DEFAULT 1,
  `closed` int(0) NOT NULL DEFAULT 0,
  `stateClosed` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_shops_items
-- ----------------------------
DROP TABLE IF EXISTS `server_shops_items`;
CREATE TABLE `server_shops_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `shopId` int(0) NOT NULL,
  `itemName` varchar(64) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `itemPrice` int(0) NOT NULL,
  `itemAmount` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_storages
-- ----------------------------
DROP TABLE IF EXISTS `server_storages`;
CREATE TABLE `server_storages`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `owner` int(0) NOT NULL,
  `secondOwner` int(0) NOT NULL,
  `entryPos` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `items` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `maxSize` float NOT NULL,
  `price` int(0) NOT NULL,
  `isfaction` int(0) NOT NULL,
  `factionid` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_tablet_apps
-- ----------------------------
DROP TABLE IF EXISTS `server_tablet_apps`;
CREATE TABLE `server_tablet_apps`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `appName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `appPrice` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_tablet_events
-- ----------------------------
DROP TABLE IF EXISTS `server_tablet_events`;
CREATE TABLE `server_tablet_events`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `title` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `owner` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `callnumber` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `location` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `eventtype` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `date` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `info` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `created` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_tablet_notes
-- ----------------------------
DROP TABLE IF EXISTS `server_tablet_notes`;
CREATE TABLE `server_tablet_notes`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charId` int(0) NOT NULL,
  `color` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `title` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `text` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `created` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_tattoos
-- ----------------------------
DROP TABLE IF EXISTS `server_tattoos`;
CREATE TABLE `server_tattoos`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `collection` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `nameHash` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `name` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `part` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `price` int(0) NOT NULL DEFAULT 15,
  `gender` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_tattooshops
-- ----------------------------
DROP TABLE IF EXISTS `server_tattooshops`;
CREATE TABLE `server_tattooshops`  (
  `id` int(0) NOT NULL,
  `name` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `owner` int(0) NOT NULL,
  `bank` int(0) NOT NULL,
  `price` int(0) NOT NULL,
  `pedX` float NOT NULL,
  `pedY` float NOT NULL,
  `pedZ` float NOT NULL,
  `pedModel` varchar(64) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pedRot` float NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_teleports
-- ----------------------------
DROP TABLE IF EXISTS `server_teleports`;
CREATE TABLE `server_teleports`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `targetX` float NOT NULL,
  `targetY` float NOT NULL,
  `targetZ` float NOT NULL,
  `dimension` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_trashcans
-- ----------------------------
DROP TABLE IF EXISTS `server_trashcans`;
CREATE TABLE `server_trashcans`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `pos` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `items` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `maxSize` float NOT NULL DEFAULT 100,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for server_vehicle_items
-- ----------------------------
DROP TABLE IF EXISTS `server_vehicle_items`;
CREATE TABLE `server_vehicle_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `vehId` int(0) NOT NULL,
  `itemName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `itemAmount` int(0) NOT NULL,
  `isInGlovebox` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_vehicle_shops
-- ----------------------------
DROP TABLE IF EXISTS `server_vehicle_shops`;
CREATE TABLE `server_vehicle_shops`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `pedX` float NOT NULL,
  `pedY` float NOT NULL,
  `pedZ` float NOT NULL,
  `pedRot` float NOT NULL,
  `pedModel` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `parkOutX` float NOT NULL,
  `parkOutY` float NOT NULL,
  `parkOutZ` float NOT NULL,
  `parkOutRotX` float NOT NULL,
  `parkOutRotY` float NOT NULL,
  `parkOutRotZ` float NOT NULL,
  `neededLicense` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'None',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_vehicle_shops_items
-- ----------------------------
DROP TABLE IF EXISTS `server_vehicle_shops_items`;
CREATE TABLE `server_vehicle_shops_items`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `shopid` int(0) NOT NULL,
  `hash` bigint(0) UNSIGNED NOT NULL,
  `price` int(0) NOT NULL,
  `isOnlyOnlineAvailable` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_vehicles
-- ----------------------------
DROP TABLE IF EXISTS `server_vehicles`;
CREATE TABLE `server_vehicles`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `charid` int(0) NOT NULL,
  `hash` bigint(0) NOT NULL,
  `vehType` int(0) NOT NULL,
  `faction` int(0) NOT NULL DEFAULT 0,
  `fuel` float NOT NULL,
  `km` float NOT NULL DEFAULT 0,
  `enginestate` tinyint(1) NOT NULL DEFAULT 0,
  `isEngineHealthy` tinyint(1) NOT NULL DEFAULT 1,
  `lockstate` tinyint(1) NOT NULL DEFAULT 0,
  `isingarage` tinyint(1) NOT NULL DEFAULT 0,
  `garageid` int(0) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotX` float NOT NULL,
  `rotY` float NOT NULL,
  `rotZ` float NOT NULL,
  `plate` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `lastUsage` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `buyDate` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `vehClass` int(0) NOT NULL,
  `serial` int(0) NOT NULL,
  `isRental` tinyint(1) NOT NULL DEFAULT 0,
  `expireRental` timestamp(0) NOT NULL DEFAULT '0000-00-00 00:00:00',
  `isInHouseGarage` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_vehicles_mod
-- ----------------------------
DROP TABLE IF EXISTS `server_vehicles_mod`;
CREATE TABLE `server_vehicles_mod`  (
  `id` int(0) NOT NULL,
  `vehId` int(0) NOT NULL,
  `colorPearl` int(0) NOT NULL DEFAULT 0,
  `headlightColor` int(0) NOT NULL DEFAULT 0,
  `spoiler` int(0) NOT NULL DEFAULT 0,
  `front_bumper` int(0) NOT NULL DEFAULT 0,
  `rear_bumper` int(0) NOT NULL DEFAULT 0,
  `side_skirt` int(0) NOT NULL DEFAULT 0,
  `exhaust` int(0) NULL DEFAULT 0,
  `frame` int(0) NOT NULL DEFAULT 0,
  `grille` int(0) NOT NULL DEFAULT 0,
  `hood` int(0) NOT NULL DEFAULT 0,
  `fender` int(0) NOT NULL DEFAULT 0,
  `right_fender` int(0) NOT NULL DEFAULT 0,
  `roof` int(0) NOT NULL DEFAULT 0,
  `engine` int(0) NOT NULL DEFAULT 0,
  `brakes` int(0) NOT NULL DEFAULT 0,
  `transmission` int(0) NOT NULL DEFAULT 0,
  `horns` int(0) NOT NULL DEFAULT 0,
  `suspension` int(0) NOT NULL DEFAULT 0,
  `armor` int(0) NOT NULL DEFAULT 0,
  `turbo` int(0) NOT NULL DEFAULT 0,
  `xenon` int(0) NOT NULL DEFAULT 0,
  `wheel_type` int(0) NOT NULL DEFAULT 0,
  `wheels` int(0) NOT NULL DEFAULT 0,
  `wheelcolor` int(0) NOT NULL DEFAULT 0,
  `plate_holder` int(0) NOT NULL DEFAULT 0,
  `trim_design` int(0) NOT NULL DEFAULT 0,
  `ornaments` int(0) NOT NULL DEFAULT 0,
  `dial_design` int(0) NOT NULL DEFAULT 0,
  `steering_wheel` int(0) NOT NULL DEFAULT 0,
  `shift_lever` int(0) NOT NULL DEFAULT 0,
  `plaques` int(0) NOT NULL DEFAULT 0,
  `hydraulics` int(0) NOT NULL DEFAULT 0,
  `airfilter` int(0) NOT NULL DEFAULT 0,
  `window_tint` int(0) NOT NULL DEFAULT 0,
  `livery` int(0) NOT NULL DEFAULT 0,
  `plate` int(0) NOT NULL DEFAULT 0,
  `neon` int(0) NOT NULL DEFAULT 0,
  `neon_r` int(0) NOT NULL DEFAULT 255,
  `neon_g` int(0) NOT NULL DEFAULT 255,
  `neon_b` int(0) NOT NULL DEFAULT 255,
  `smoke_r` int(0) NOT NULL DEFAULT 0,
  `smoke_g` int(0) NOT NULL DEFAULT 0,
  `smoke_b` int(0) NOT NULL DEFAULT 0,
  `back_wheels` int(0) NOT NULL DEFAULT 0,
  `plate_vanity` int(0) NOT NULL DEFAULT 0,
  `door_interior` int(0) NOT NULL DEFAULT 0,
  `seats` int(0) NOT NULL DEFAULT 0,
  `rear_shelf` int(0) NOT NULL DEFAULT 0,
  `trunk` int(0) NOT NULL DEFAULT 0,
  `engine_block` int(0) NOT NULL DEFAULT 0,
  `strut_bar` int(0) NOT NULL DEFAULT 0,
  `arch_cover` int(0) NOT NULL DEFAULT 0,
  `antenna` int(0) NOT NULL DEFAULT 0,
  `exterior_parts` int(0) NOT NULL DEFAULT 0,
  `tank` int(0) NOT NULL DEFAULT 0,
  `rear_hydraulics` int(0) NOT NULL DEFAULT 0,
  `door` int(0) NOT NULL DEFAULT 0,
  `plate_color` int(0) NOT NULL DEFAULT 0,
  `colorPrimary_r` int(0) NOT NULL DEFAULT 0,
  `colorPrimary_g` int(0) NOT NULL DEFAULT 0,
  `colorPrimary_b` int(0) NOT NULL DEFAULT 0,
  `colorSecondary_r` int(0) NOT NULL DEFAULT 0,
  `colorSecondary_g` int(0) NOT NULL DEFAULT 0,
  `colorSecondary_b` int(0) NOT NULL DEFAULT 0,
  `colorPrimaryType` int(0) NOT NULL DEFAULT 0,
  `colorSecondaryType` int(0) NOT NULL DEFAULT 0,
  `interior_color` int(0) NOT NULL DEFAULT 0,
  `smoke` int(0) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_wanteds
-- ----------------------------
DROP TABLE IF EXISTS `server_wanteds`;
CREATE TABLE `server_wanteds`  (
  `wantedId` int(0) NOT NULL AUTO_INCREMENT,
  `category` int(0) NOT NULL,
  `wantedName` varchar(128) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `paragraph` int(0) NOT NULL,
  `jailtime` int(0) NOT NULL,
  `ticketfine` int(0) NOT NULL,
  PRIMARY KEY (`wantedId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for server_weedpots
-- ----------------------------
DROP TABLE IF EXISTS `server_weedpots`;
CREATE TABLE `server_weedpots`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `remainingMinutes` int(0) NOT NULL,
  `water` int(0) NOT NULL,
  `hasFertilizer` tinyint(1) NOT NULL,
  `position` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `dimension` int(0) NOT NULL,
  `state` int(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Table structure for support
-- ----------------------------
DROP TABLE IF EXISTS `support`;
CREATE TABLE `support`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `msg` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `bug` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posted` datetime(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tweets
-- ----------------------------
DROP TABLE IF EXISTS `tweets`;
CREATE TABLE `tweets`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `msg` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `liked` int(0) NOT NULL DEFAULT 0,
  `posted` timestamp(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `id` bigint(0) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `email_verified_at` timestamp(0) NULL DEFAULT NULL,
  `password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `remember_token` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `created_at` timestamp(0) NULL DEFAULT NULL,
  `updated_at` timestamp(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `users_email_unique`(`email`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for whitelist
-- ----------------------------
DROP TABLE IF EXISTS `whitelist`;
CREATE TABLE `whitelist`  (
  `id` int(0) NOT NULL AUTO_INCREMENT,
  `username` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `msg` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `posted` datetime(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 0 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Event structure for parkVehicles
-- ----------------------------
DROP EVENT IF EXISTS `parkVehicles`;
delimiter ;;
CREATE EVENT `parkVehicles`
ON SCHEDULE
EVERY '24' HOUR STARTS '2021-10-25 23:59:44'
DO UPDATE server_vehicles SET isingarage = 0 WHERE lastUsage < DATE_SUB(NOW(), INTERVAL 3 DAY) AND isingarage = 0
;
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
