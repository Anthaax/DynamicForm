using System;
using System.Collections.Generic;
using DynamicForm.Core.Interfaces;

namespace DynamicForm.Core
{
    public abstract class QuestionBase
    {
        QuestionBase _parent;
        string _title;
        public QuestionBase(QuestionBase parent)
        {
            if (parent == null) throw new ArgumentNullException(" Parent can't be null.");
            _childrens = new List<QuestionBase>();
            _parent = parent;
        }
        internal QuestionBase()
        {
            _childrens = new List<QuestionBase>();
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

        public bool Contains(QuestionBase child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            QuestionBase q = child;
            while(q.Parent != null)
            {
                if (q.Parent == this) return true;
                q = q.Parent;
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

        public IQuestionChildrenList Children => new QuestionChildrensList(_childrens);

        public virtual Form Form => Parent?.Form;

        public abstract AnswerBase AnswerModel { get; }
    }
}