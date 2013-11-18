using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using amulware.Graphics.utilities;

namespace amulware.Graphics.Animation
{
    sealed public class SkeletonTemplate
    {
        private readonly ReadOnlyCollection<BoneTemplate> bones;
        private readonly Dictionary<string, BoneTemplate> boneDictionary;

        internal SkeletonTemplate(IEnumerable<BoneJsonRepresentation> bones)
        {
            var boneList = new List<BoneTemplate>();
            this.bones = boneList.AsReadOnly();
            this.boneDictionary = new Dictionary<string, BoneTemplate>();

            // ReSharper disable PossibleMultipleEnumeration
            // see assignment of leftOverBones to bones at end of loop below
            if (bones.IsNullOrEmpty())
                return;

            int id = 0;

            List<BoneJsonRepresentation> leftOverBones;
            bool pickedUpBone;

            // loop over bones repeatedly until all parent-child relations
            // are found (and all bones picked up) or unreachable bones are detected
            // as a result boneList and this.bones will contain all bones partially ordered
            // from spines to extremeties
            // performs in O(n) if bones are given in such an order, up to O(n^2) if bones
            // are given in reverse
            do
            {
                leftOverBones = new List<BoneJsonRepresentation>();
                pickedUpBone = false;
                foreach (var bone in bones)
                {
                    BoneTemplate parent = null;
                    if (!string.IsNullOrEmpty(bone.Parent) &&
                        !this.boneDictionary.TryGetValue(bone.Parent, out parent))
                    {
                        // found bone with unknown parent
                        leftOverBones.Add(bone);
                        continue;
                    }
                    var b = new BoneTemplate(id++, bone.Name, parent, bone.Sprite);
                    boneList.Add(b);
                    this.boneDictionary.Add(bone.Name, b);
                    pickedUpBone = true;
                }
                bones = leftOverBones;
            } while (leftOverBones.Count > 0 && pickedUpBone);

            // ReSharper restore PossibleMultipleEnumeration

            if (leftOverBones.Count > 0)
            {
                if (leftOverBones.Count == 1)
                    throw new InvalidDataException("Found 1 bone with unknown parent.");
                throw new InvalidDataException(
                    string.Format("Found {0} bones with unknown parents.", leftOverBones.Count));
            }

            boneList.TrimExcess();
        }

        public BoneTemplate this[int boneId]
        {
            get { return this.bones[boneId]; }
        }

        public BoneTemplate this[string boneName]
        {
            get { return this.boneDictionary[boneName]; }
        }

        public ReadOnlyCollection<BoneTemplate> Bones
        {
            get { return this.bones; }
        } 
    }
}
