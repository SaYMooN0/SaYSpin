﻿namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseGameplayItem
    {
        public abstract string Id { get; init; }
        public virtual string Image { get; init; } = "default.png";
        public abstract string ImageFolderPath { get; }
    }
}
