namespace SmartSchoolLifeAPI.Core.Models.Attendance
{
    public class AbsenceReasonsModel
    {
        public int AbsenceReasonID { get; set; }
        public string AbsenceReasonArabicText { get; set; }
        public string AbsenceReasonEnglishText { get; set; }

        public bool ShouldSerializeAbsenceReasonID() => AbsenceReasonID != default(int);
        public bool ShouldSerializeAbsenceReasonArabicText() => !string.IsNullOrEmpty(AbsenceReasonArabicText);
        public bool ShouldSerializeAbsenceReasonEnglishText() => !string.IsNullOrEmpty(AbsenceReasonEnglishText);
    }
}