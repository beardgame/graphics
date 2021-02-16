using System.Collections.Immutable;
using System.Linq;

namespace Bearded.Graphics.Text
{
    public sealed class Font
    {
        private readonly ImmutableDictionary<char, CharacterInfo> characters;

        public CharacterInfo FallbackCharacter { get; }

        public Font(ImmutableDictionary<char, CharacterInfo> characters, CharacterInfo fallbackCharacter)
        {
            this.characters = characters;
            FallbackCharacter = fallbackCharacter;
        }

        public CharacterInfo GetCharacterInfoFor(char character)
        {
            return TryGetCharacterInfoFor(character, out var info) ? info : FallbackCharacter;
        }

        public bool TryGetCharacterInfoFor(char character, out CharacterInfo characterInfo)
        {
            if (characters.TryGetValue(character, out var foundValue))
            {
                characterInfo = foundValue;
                return true;
            }

            characterInfo = default;
            return false;
        }

        public float StringWidth(string text)
        {
            return text.Sum(c => GetCharacterInfoFor(c).SpacingWidth);
        }
    }
}
