using System;
using System.Collections.Generic;

namespace DynamicForm.Core
{
    public class Form
    {
        string _title;
        public Form()
        {
            Questions = new QuestionRoot(this);
        }
        public string Title { get { return _title; } set { _title = value; } }
        public int AnswerCount => _answer.Count;
        public QuestionBase Questions { get; set; }
        readonly Dictionary<string, FormAnswer> _answer = new Dictionary<string, FormAnswer>();

        public FormAnswer FindOrCreateAnswer(string name)
        {
            FormAnswer fa;
            if(!_answer.TryGetValue(name, out fa))
            {
                fa = CreateAnswer(name);
            }
            return fa;
        }

        public FormAnswer CreateAnswer(string name)
        {
            FormAnswer fa = new FormAnswer(name);
            _answer.Add(name, fa);
            return fa;
        }

        public FormAnswer FindAnswer(string name)
        {
            FormAnswer fa;
            _answer.TryGetValue(name, out fa);
            return fa;

        }
    }
}