﻿using SaYSpin.src.static_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related.tokens
{
    public class TokensCollection
    {
        private Dictionary<TokenType, int> _tokens = new Dictionary<TokenType, int>();

        public TokensCollection(
            int shopRefreshTokensCount = 0,
            int tileItemRemovalTokensCount = 0,
            int newStageItemsRefreshTokensCount = 0)
        {
            _tokens[TokenType.ShopRefresh] = shopRefreshTokensCount;
            _tokens[TokenType.InventoryItemRemoval] = tileItemRemovalTokensCount;
            _tokens[TokenType.NewStageItemsRefresh] = newStageItemsRefreshTokensCount;
        }
        public int Count(TokenType tokenType) => _tokens[tokenType];

        public bool TryUseToken(TokenType tokenType)
        {
            if (_tokens[tokenType] > 0)
            {
                _tokens[tokenType]--;
                TokensCountChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void AddToken(TokenType tokenType)
        {
            _tokens[tokenType]++;
            TokensCountChanged?.Invoke();
        }

        static public TokenType RandomTokenType()
        {
            var tokenTypes = Enum.GetValues(typeof(TokenType));
            var randomIndex = Randomizer.Int(tokenTypes.Length);
            return (TokenType)tokenTypes.GetValue(randomIndex);
        }
        public IEnumerable<(TokenType tokenType, int count)> TokensAsTuples()
        {
            foreach (var kvp in _tokens)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }
        public static IEnumerable<TokenType> AllTokenTypes =>
            (TokenType[])Enum.GetValues(typeof(TokenType));
        internal void Clear() =>
            _tokens = _tokens.ToDictionary(kvp => kvp.Key, kvp => 0);
        public event Action TokensCountChanged;
    }

}
