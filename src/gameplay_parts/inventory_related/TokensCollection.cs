using System;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class TokensCollection
    {
        private Dictionary<TokenType, int> _tokens = new Dictionary<TokenType, int>();

        public TokensCollection(
            int addSpinTokensCount = 0,
            int freeShopRefreshTokensCount = 0,
            int tileItemRemovalTokensCount = 0,
            int newStageRelicChoiceRefreshTokensCount = 0,
            int newStageItemChoiceRefreshTokensCount = 0)
        {
            _tokens[TokenType.AddSpin] = addSpinTokensCount;
            _tokens[TokenType.FreeShopRefresh] = freeShopRefreshTokensCount;
            _tokens[TokenType.TileItemRemoval] = tileItemRemovalTokensCount;
            _tokens[TokenType.NewStageRelicChoiceRefresh] = newStageRelicChoiceRefreshTokensCount;
            _tokens[TokenType.NewStageItemChoiceRefresh] = newStageItemChoiceRefreshTokensCount;
        }

        public enum TokenType
        {
            AddSpin,
            FreeShopRefresh,
            TileItemRemoval,
            NewStageRelicChoiceRefresh,
            NewStageItemChoiceRefresh
        }

        public int GetTokenCount(TokenType tokenType) => _tokens[tokenType];

        public bool TryUseToken(TokenType tokenType)
        {
            if (_tokens[tokenType] > 0)
            {
                _tokens[tokenType]--;
                return true;
            }
            return false;
        }

        public void AddToken(TokenType tokenType)=>
            _tokens[tokenType]++;
        static public TokenType GetRandomTokenType()
        {
            var tokenTypes = Enum.GetValues(typeof(TokenType)); 
            var randomIndex = Randomizer.Int(tokenTypes.Length); 
            return (TokenType)tokenTypes.GetValue(randomIndex);
        }
    }

}
