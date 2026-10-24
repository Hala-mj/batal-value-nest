using UnityEngine;
using System;

namespace UI.Dates
{
    [Serializable]
    public struct SerializableDate
    {
        public DateTime Date
        {
            get
            {
                return DateTime.ParseExact(m_SerializedDate, DatePickerUtilities.DateFormat, null);
            }
            set
            {
                m_SerializedDate = value.ToString(DatePickerUtilities.DateFormat);
            }
        }

        [SerializeField]
        private string m_SerializedDate;

#if UNITY_EDITOR
#pragma warning disable 414
        [SerializeField]
        private bool m_showEditorCalendar;
#pragma warning restore 414
#endif

        public bool HasValue
        {
            get
            {
                return !string.IsNullOrEmpty(m_SerializedDate);
            }
        }

        public SerializableDate(DateTime date)
        {
#if UNITY_EDITOR
            m_showEditorCalendar = false;
#endif
            m_SerializedDate = "";
            Date = date;
        }

        public SerializableDate(SerializableDate date)
        {
#if UNITY_EDITOR
            m_showEditorCalendar = false;
#endif
            m_SerializedDate = "";
            if (date.HasValue)
            {
                Date = date.Date;
            }
        }

        /// <summary>
        /// Returns the date in the specified format.
        /// </summary>
        public string ToFormattedString(string format)
        {
            if (HasValue)
            {
                return Date.ToString(format);
            }
            return null;
        }

        /// <summary>
        /// Overrides the default ToString() method.
        /// </summary>
        public override string ToString()
        {
            if (HasValue)
            {
                return Date.ToString(DatePickerUtilities.DateFormat);
            }
            return "No Date";
        }

        #region Operators
        public static implicit operator SerializableDate(DateTime date)
        {
            return new SerializableDate(date);
        }

        public static implicit operator DateTime(SerializableDate date)
        {
            return date.Date;
        }
        #endregion
    }
}
