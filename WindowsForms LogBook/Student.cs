using System;

namespace WindowsForms_LogBook
{
    public class Student : Person
    {
        public Student(string fullName, DateTime lastTrainingTime, string state, string examinationWorkGrade, string ClassworkGrade, int crystalCount, string commentFromTeacher)
        {
            FullName = fullName;
            LastTrainingTime = DateTime.Now;
            State = state;
            ExaminationWorkGrade = examinationWorkGrade;
            ClassWorkGrade = ClassworkGrade;
            CrystalCount = crystalCount;
            CommentFromTrainer = commentFromTeacher;
        } 
        public string FullName { get; set; }
        public DateTime LastTrainingTime { get; set; } = DateTime.Now;
        public string State { get; set; } = AttentionStates.None;

        public string ExaminationWorkGrade { get; set; }
        public string ClassWorkGrade { get; set; }
        public int CrystalCount { get; set; } = default;

        public string CommentFromTrainer { get; set; } = default;

    }
}
