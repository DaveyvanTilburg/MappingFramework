﻿using System.Collections.Generic;
using System.Linq;
using MappingFramework.Process;

namespace MappingFramework.TDD
{
    internal class TestErrorObserver : ProcessObserver
    {
        private readonly List<Information> _information = new List<Information>();

        public IReadOnlyCollection<Information> GetRaisedWarnings()
        {
            return _information.Where(i => i.Type.Equals("warning")).ToList();
        }

        public IReadOnlyCollection<Information> GetRaisedErrors()
        {
            return _information.Where(i => i.Type.Equals("error")).ToList();
        }

        public IReadOnlyCollection<Information> GetRaisedOtherTypes()
        {
            return _information.Where(i => !i.Type.Equals("error") && !i.Type.Equals("warning")).ToList();
        }

        public List<Information> GetInformation()
        {
            return _information;
        }
        
        public void InformationRaised(Information information)
        {
            _information.Add(information);
        }

        public void Register()
        {
            ProcessObservable.GetInstance().Register(this);
        }

        public void Unregister()
        {
            ProcessObservable.GetInstance().Unregister(this);
        }
    }
}