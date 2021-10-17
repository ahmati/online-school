using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineSchool.Contract.Infrastructure
{
    /// <summary>
    /// Creo una semplice classe base da usare nelle varie risposte così se in futuro abbiamo bisogno di gestire esempio
    /// enum di risposte possiamo modificare la classe base senza dover modificare tutti i metodi
    /// </summary>
    public class ResponseBase<T>
    {
        public T Output { get; set; }
        public IList<string> Errors { get; private set; } = new List<string>();
        public bool HasErrors => Errors != null && Errors.Any();
        public void AddError(params string[] errors)
        {
            if (Errors == null)
                Errors = new List<string>();

            foreach (var e in errors)
            {
                Errors.Add(e);
            }
        }
        public void AddErrors(IEnumerable<string> errors)
        {
            foreach(var error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}
