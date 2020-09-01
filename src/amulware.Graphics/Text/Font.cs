using System.Collections.Immutable;
using System.Linq;

namespace amulware.Graphics.Text
{
    public sealed class Font
    {
        private readonly ImmutableDictionary<char, CharacterInfo> characters;

        public Font(ImmutableDictionary<char, CharacterInfo> characters)
        {
            this.characters = characters;
        }

        public CharacterInfo GetCharacterInfoFor(char character)
        {
            return characters[character];
        }

        public float StringWidth(string text)
        {
            return text.Sum(c => GetCharacterInfoFor(c).SpacingWidth);
        }
    }
}
