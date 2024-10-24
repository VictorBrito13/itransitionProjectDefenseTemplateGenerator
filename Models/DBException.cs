using System.ComponentModel.DataAnnotations;

namespace ItransitionTemplates.Models {

    public enum DBExceptionType {
        DuplicateEntry,
        NullValue,
        UnknownException
    }

    public class DBException : Exception {
        [Required]
        public string Msg { get; }
        public DBExceptionType Type { get; }

        public DBException(string msg, DBExceptionType type) {
            Msg = msg;
            Type = type;
        }
    }
}