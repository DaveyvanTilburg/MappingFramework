﻿using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelGetValueTraversal : GetValueTraversal
    {
        public ModelGetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if(!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#16; source is not of expected type Model", "error", Path, source);
                return string.Empty;
            }

            var modelPathContainer = PathContainer.Create(Path);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            if (!pathTarget.IsValid())
                return string.Empty;

            string value = pathTarget.GetValue(modelPathContainer.LastInPath);
            return value;
        }
    }
}