using System;
using System.Collections.Generic;

namespace DynamicForm.Core
{
    public abstract class QuestionBase
    {
        QuestionBase _parent;
        string _title;
        public QuestionBase(QuestionBase parent)
        {
            _childrens = new List<QuestionBase>();
            _parent = parent;
        }

        public int Index => _parent != null ? 
                            _parent._childrens.IndexOf(this) 
                            : -1;
        private readonly List<QuestionBase> _childrens;
        public virtual QuestionBase Parent
        {
            get => _parent;
            set
            {
                if (_parent != value)
                {
                    if (_parent != null)
                    {
                        _parent._childrens.Remove(this);
                    }
                    _parent = value;
                    if (_parent != null)
                    {
                        _parent._childrens.Add(this);
                    }
                }
            }
        }
        public virtual string Title { get { return _title; } set { _title = value; } }

        public void SetIndex(int index)
        {
            if (Parent == null) throw new InvalidOperationException();
            if (index < 0 || index > Parent._childrens.Count) throw new IndexOutOfRangeException();

            int initialIndex = Index;
            _parent._childrens.Insert(index,this);
            if (index < initialIndex)
                _parent._childrens.RemoveAt(initialIndex + 1);
            else
                _parent._childrens.RemoveAt(index);
        }

        public bool Contains(QuestionBase question)
        {
            if (_childrens.Count == 0) return false;
            if (_childrens.Contains(question)) return true;
            foreach (var child in _childrens)
            {
                if (child.Contains(question)) return true;
            }
            return false;
        }

        public QuestionBase AddNewQuestion(string type)
        {
            Type tQuestion = Type.GetType(type);
            return AddNewQuestion(tQuestion);
        }

        public QuestionBase AddNewQuestion(Type type)
        {
            var qObject = (QuestionBase)Activator.CreateInstance(type, this);
            _childrens.Add(qObject);
            return qObject;
        }

        public IReadOnlyList<QuestionBase> Children => _childrens;

        public virtual Form Form => Parent?.Form;

        public virtual AnswerBase AnswerModel { get { return new AnswerBase(this); } }
    }
}