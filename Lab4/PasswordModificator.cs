namespace Lab4
{
    using System.Text;

    public abstract class PasswordModificator
    {
        public abstract void Modify(StringBuilder builder);
        public abstract float Weight { get; }
    }

    public class UpperCaseModificator : PasswordModificator
    {
        private const int MaxAmount = 3;
        public override float Weight => 0.2f;

        public override void Modify(StringBuilder builder)
        {
            var length = builder.Length;
            var charactersAmount = PasswordsGenerator.Random.Next(MaxAmount) + 1;

            for (var i = 0; i < charactersAmount; i++)
            {
                var characterIndex = PasswordsGenerator.Random.Next(length);
                builder[characterIndex] = char.ToUpper(builder[characterIndex]);
            }
        }
    }

    public class LowerCaseModificator : PasswordModificator
    {
        private const int MaxAmount = 5;
        public override float Weight => 0.1f;

        public override void Modify(StringBuilder builder)
        {
            var length = builder.Length;
            var charactersAmount = PasswordsGenerator.Random.Next(MaxAmount) + 1;

            for (var i = 0; i < charactersAmount; i++)
            {
                var characterIndex = PasswordsGenerator.Random.Next(length);
                builder[characterIndex] = char.ToLower(builder[characterIndex]);
            }
        }
    }

    public class ReverseModificator : PasswordModificator
    {
        public override float Weight => 0.05f;

        public override void Modify(StringBuilder builder)
        {
            var end = builder.Length - 1;
            var start = 0;

            while (end - start > 0)
            {
                (builder[end], builder[start]) = (builder[start], builder[end]);
                start++;
                end--;
            }
        }
    }

    public class ReplaceCharacterModificator : PasswordModificator
    {
        private const string PossibleCharacters =
            "abcdefghijklmnopqrstuvwxyz123456789!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~";

        private const int MaxAmount = 5;
        public override float Weight => 0.1f;

        public override void Modify(StringBuilder builder)
        {
            var charactersAmount = PasswordsGenerator.Random.Next(MaxAmount) + 1;

            for (var i = 0; i < charactersAmount; i++)
            {
                var characterIndex = PasswordsGenerator.Random.Next(builder.Length);
                var newCharacterIndex = PasswordsGenerator.Random.Next(PossibleCharacters.Length);
                builder[characterIndex] = PossibleCharacters[newCharacterIndex];
            }
        }
    }

    public class AddCharacterModificator : PasswordModificator
    {
        private const string PossibleCharacters =
            "abcdefghijklmnopqrstuvwxyz123456789!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~";

        private const int MaxAmount = 3;
        public override float Weight => 0.5f;

        public override void Modify(StringBuilder builder)
        {
            var charactersAmount = PasswordsGenerator.Random.Next(MaxAmount) + 1;

            for (var i = 0; i < charactersAmount; i++)
            {
                var characterIndex = PasswordsGenerator.Random.Next(PossibleCharacters.Length);
                var insertPosition = PasswordsGenerator.Random.Next(builder.Length);
                builder.Insert(insertPosition, PossibleCharacters[characterIndex]);
            }
        }
    }

    public class AddNumbersInEndModificator : PasswordModificator
    {
        private const int MaxAmount = 5;
        public override float Weight => 0.5f;
        private const string PossibleCharacters = "123456789";

        public override void Modify(StringBuilder builder)
        {
            var charactersAmount = PasswordsGenerator.Random.Next(MaxAmount) + 1;

            for (var i = 0; i < charactersAmount; i++)
            {
                var characterIndex = PasswordsGenerator.Random.Next(PossibleCharacters.Length);
                builder.Append(PossibleCharacters[characterIndex]);
            }
        }
    }
}