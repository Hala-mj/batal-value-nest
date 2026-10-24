-- BVN (Batal Value Nest) database schema
-- Exported from phpMyAdmin (MariaDB 10.4 / XAMPP).
-- Sample user, child, message, reward and schedule rows were removed;
-- reference data (values, tasks, icons) is kept.
--
-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 02, 2025 at 11:50 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bvn`
--

-- --------------------------------------------------------

--
-- Table structure for table `batalvalues`
--

CREATE TABLE `batalvalues` (
  `ValueID` int(11) NOT NULL,
  `ValueName` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `batalvalues`
--

INSERT INTO `batalvalues` (`ValueID`, `ValueName`) VALUES
(1, 'المواظبة'),
(2, 'المسؤولية'),
(3, 'النظافة'),
(4, 'العطف'),
(5, 'الايثار'),
(6, 'الصدق'),
(7, 'الكرم');

-- --------------------------------------------------------

--
-- Table structure for table `challengeparticipants`
--

CREATE TABLE `challengeparticipants` (
  `ParticipantID` int(11) NOT NULL,
  `ChallengeID` int(11) DEFAULT NULL,
  `ChildID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `challenges`
--

CREATE TABLE `challenges` (
  `ChallengeID` int(11) NOT NULL,
  `ValueID` int(11) DEFAULT NULL,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `child`
--

CREATE TABLE `child` (
  `ChildID` int(11) NOT NULL,
  `ParentID` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  `IconID` int(11) DEFAULT NULL,
  `Coins` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `dailytasks`
--

CREATE TABLE `dailytasks` (
  `TaskID` int(11) NOT NULL,
  `ScheduleID` int(11) DEFAULT NULL,
  `ChildID` int(11) DEFAULT NULL,
  `TaskState` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `icons`
--

CREATE TABLE `icons` (
  `IconID` int(11) NOT NULL,
  `source` varchar(255) DEFAULT NULL,
  `type` enum('AvatarIcon','RewardIcon') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `icons`
--

INSERT INTO `icons` (`IconID`, `source`, `type`) VALUES
(1, 'http://localhost/BVN/images/avatars/Rabbit.png', 'AvatarIcon'),
(2, '/BVN/images/avatars/Dog.png', 'AvatarIcon'),
(3, '/BVN/images/avatars/Cat.png', 'AvatarIcon'),
(4, '/BVN/images/avatars/Bird.png', 'AvatarIcon'),
(5, 'http://localhost/BVN/Icons/balling.png', 'RewardIcon'),
(6, 'http://localhost/BVN/Icons/star.png', 'RewardIcon'),
(7, 'http://localhost/BVN/Icons/aerobicQube.png', 'RewardIcon'),
(8, 'http://localhost/BVN/Icons/conrtroller.png', 'RewardIcon'),
(9, 'http://localhost/BVN/Icons/tickets.png', 'RewardIcon'),
(10, 'http://localhost/BVN/Icons/ball.png', 'RewardIcon');

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE `messages` (
  `MessageID` int(11) NOT NULL,
  `ChildID` int(11) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `TextContent` text DEFAULT NULL,
  `DOR` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `reward`
--

CREATE TABLE `reward` (
  `RewardID` int(11) NOT NULL,
  `ChilledID` int(11) DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `ValueID` int(11) DEFAULT NULL,
  `IconID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `scheduled`
--

CREATE TABLE `scheduled` (
  `ScheduledID` int(11) NOT NULL,
  `TaskID` int(11) DEFAULT NULL,
  `ChilledID` int(11) DEFAULT NULL,
  `Duration` int(11) DEFAULT NULL,
  `Deadline` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `task`
--

CREATE TABLE `task` (
  `TaskID` int(11) NOT NULL,
  `TaskName` varchar(255) DEFAULT NULL,
  `ValueID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `task`
--

INSERT INTO `task` (`TaskID`, `TaskName`, `ValueID`) VALUES
(1, 'قراءة صفحة من القرآن', 1),
(2, 'تجهيز ملابس اليوم التالي', 1),
(3, 'مراجعة الدروس', 1),
(4, 'ترتيب حقيبة المدرسة كل مساء', 1),
(5, 'رياضة الصباح', 1),
(6, 'سقي النباتات بإنتظام', 2),
(7, 'تنظيم الألعاب بعد الانتهاء من اللعب', 2),
(8, 'المساعدة في ترتيب المائدة', 2),
(9, 'وضع الأكل للحيوان الأليف', 2),
(10, 'إطفاء النور عند الخروج من الغرفة', 2),
(11, 'غسل اليدين بعد الأكل', 3),
(12, 'ترتيب السرير', 3),
(13, 'تنظيف الأسنان صبح ومساء', 3),
(14, 'تغيير الملابس الداخلية', 3),
(15, 'تنظيف الطاولة بعد الطعام', 3),
(16, 'إرسال رسالة لطيفة لصديق وتقديم الشكر', 4),
(17, 'التصدق على محتاج', 4),
(18, 'مشاركة الألعاب مع الأصدقاء', 4),
(19, 'فتح الباب لشخص آخر', 4),
(20, 'التنازل عن وقتك في مشاهدة التلفاز لأحد أفراد الأسرة', 5),
(21, 'مشاركة أدوات المدرسة مع زميل نسي أدواته في الحصة', 5),
(22, 'إعطاء الدور في الأرجوحة لطفل آخر', 5),
(23, 'تخصيص وقت للعب مع أختك الصغير بدل استخدام الجوال', 5),
(24, 'ترك مكانك المفضل لأحد الضيوف', 5),
(25, 'الاعتراف إذا لم تكمل المهمة', 6),
(26, 'الاعتراف إذا كسرت شيء بالمنزل', 6),
(27, 'إخبار الأهل إذا واجه موقفًا مزعجًا في المدرسة', 6),
(28, 'الإجابة بصدق عند سؤال الوالدين عن يومه بالمدرسة', 6),
(29, 'التحدث بصدق مع الإخوة إذا أخذت شيئًا من ممتلكاتهم دون إذن', 6),
(30, 'قدم هدية بسيطة لصديقك المفضل', 7),
(31, 'مساعدة أحد العمال بإحضار كوب ماء', 7),
(32, 'توزيع التمر أو الحلوى على أصدقائك بالفصل', 7),
(33, 'تجهيز ساندويتش إضافي لمشاركته مع زميلك في المدرسة', 7),
(34, 'إهداء عمل فني قمت به لأحد أفراد عائلتك', 7);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `ParentID` int(11) NOT NULL,
  `Fname` varchar(255) DEFAULT NULL,
  `Lname` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `PhoneNo` varchar(255) DEFAULT NULL,
  `Classification` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `batalvalues`
--
ALTER TABLE `batalvalues`
  ADD PRIMARY KEY (`ValueID`);

--
-- Indexes for table `challengeparticipants`
--
ALTER TABLE `challengeparticipants`
  ADD PRIMARY KEY (`ParticipantID`),
  ADD KEY `ChallengeID` (`ChallengeID`),
  ADD KEY `ChildID` (`ChildID`);

--
-- Indexes for table `challenges`
--
ALTER TABLE `challenges`
  ADD PRIMARY KEY (`ChallengeID`),
  ADD KEY `ValueID` (`ValueID`);

--
-- Indexes for table `child`
--
ALTER TABLE `child`
  ADD PRIMARY KEY (`ChildID`),
  ADD KEY `FK_Chiled_Parent` (`ParentID`),
  ADD KEY `FK_Chiled_Icon` (`IconID`);

--
-- Indexes for table `dailytasks`
--
ALTER TABLE `dailytasks`
  ADD PRIMARY KEY (`TaskID`),
  ADD KEY `fk_daily_tasks_schedule` (`ScheduleID`),
  ADD KEY `fk_daily_tasks_child` (`ChildID`);

--
-- Indexes for table `icons`
--
ALTER TABLE `icons`
  ADD PRIMARY KEY (`IconID`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`MessageID`),
  ADD KEY `FK_Messages_Chiled` (`ChildID`);

--
-- Indexes for table `reward`
--
ALTER TABLE `reward`
  ADD PRIMARY KEY (`RewardID`),
  ADD KEY `FK_Reward_Chiled` (`ChilledID`),
  ADD KEY `FK_Reward_Value` (`ValueID`),
  ADD KEY `FK_Reward_Icon` (`IconID`);

--
-- Indexes for table `scheduled`
--
ALTER TABLE `scheduled`
  ADD PRIMARY KEY (`ScheduledID`),
  ADD KEY `FK_Scheduled_Task` (`TaskID`),
  ADD KEY `FK_Scheduled_Chiled` (`ChilledID`);

--
-- Indexes for table `task`
--
ALTER TABLE `task`
  ADD PRIMARY KEY (`TaskID`),
  ADD KEY `FK_Task_Value` (`ValueID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`ParentID`),
  ADD UNIQUE KEY `PhoneNo` (`PhoneNo`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `batalvalues`
--
ALTER TABLE `batalvalues`
  MODIFY `ValueID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `challengeparticipants`
--
ALTER TABLE `challengeparticipants`
  MODIFY `ParticipantID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `challenges`
--
ALTER TABLE `challenges`
  MODIFY `ChallengeID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `child`
--
ALTER TABLE `child`
  MODIFY `ChildID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `dailytasks`
--
ALTER TABLE `dailytasks`
  MODIFY `TaskID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `icons`
--
ALTER TABLE `icons`
  MODIFY `IconID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `MessageID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reward`
--
ALTER TABLE `reward`
  MODIFY `RewardID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `scheduled`
--
ALTER TABLE `scheduled`
  MODIFY `ScheduledID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `task`
--
ALTER TABLE `task`
  MODIFY `TaskID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `ParentID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `challengeparticipants`
--
ALTER TABLE `challengeparticipants`
  ADD CONSTRAINT `challengeparticipants_ibfk_1` FOREIGN KEY (`ChallengeID`) REFERENCES `challenges` (`ChallengeID`),
  ADD CONSTRAINT `challengeparticipants_ibfk_2` FOREIGN KEY (`ChildID`) REFERENCES `child` (`ChildID`);

--
-- Constraints for table `challenges`
--
ALTER TABLE `challenges`
  ADD CONSTRAINT `challenges_ibfk_1` FOREIGN KEY (`ValueID`) REFERENCES `batalvalues` (`ValueID`);

--
-- Constraints for table `child`
--
ALTER TABLE `child`
  ADD CONSTRAINT `FK_Chiled_Icon` FOREIGN KEY (`IconID`) REFERENCES `icons` (`IconID`),
  ADD CONSTRAINT `FK_Chiled_Parent` FOREIGN KEY (`ParentID`) REFERENCES `users` (`ParentID`);

--
-- Constraints for table `dailytasks`
--
ALTER TABLE `dailytasks`
  ADD CONSTRAINT `fk_daily_tasks_child` FOREIGN KEY (`ChildID`) REFERENCES `child` (`ChildID`),
  ADD CONSTRAINT `fk_daily_tasks_schedule` FOREIGN KEY (`ScheduleID`) REFERENCES `scheduled` (`ScheduledID`);

--
-- Constraints for table `messages`
--
ALTER TABLE `messages`
  ADD CONSTRAINT `FK_Messages_Chiled` FOREIGN KEY (`ChildID`) REFERENCES `child` (`ChildID`);

--
-- Constraints for table `reward`
--
ALTER TABLE `reward`
  ADD CONSTRAINT `FK_Reward_Chiled` FOREIGN KEY (`ChilledID`) REFERENCES `child` (`ChildID`),
  ADD CONSTRAINT `FK_Reward_Icon` FOREIGN KEY (`IconID`) REFERENCES `icons` (`IconID`),
  ADD CONSTRAINT `FK_Reward_Value` FOREIGN KEY (`ValueID`) REFERENCES `batalvalues` (`ValueID`);

--
-- Constraints for table `scheduled`
--
ALTER TABLE `scheduled`
  ADD CONSTRAINT `FK_Scheduled_Chiled` FOREIGN KEY (`ChilledID`) REFERENCES `child` (`ChildID`),
  ADD CONSTRAINT `FK_Scheduled_Task` FOREIGN KEY (`TaskID`) REFERENCES `task` (`TaskID`);

--
-- Constraints for table `task`
--
ALTER TABLE `task`
  ADD CONSTRAINT `FK_Task_Value` FOREIGN KEY (`ValueID`) REFERENCES `batalvalues` (`ValueID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
