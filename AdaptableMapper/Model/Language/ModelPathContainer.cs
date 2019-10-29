﻿using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Model.Language
{
    internal class ModelPathContainer
    {
        private readonly IReadOnlyList<string> _path;
        public string PropertyName { get; }
        public Queue<string> CreatePathQueue()
        {
            return new Queue<string>(_path);
        }

        private ModelPathContainer(IReadOnlyList<string> path, string propertyName)
        {
            _path = path;
            PropertyName = propertyName;
        }

        public static ModelPathContainer CreateModelPath(string modelPath)
        {
            Stack<string> pathStack = modelPath.ToStack();
            string propertyName = pathStack.Pop();

            var path = pathStack.Reverse().ToList();

            return new ModelPathContainer(path, propertyName);
        }
    }
}