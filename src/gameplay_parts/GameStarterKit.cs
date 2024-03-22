using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;

namespace SaYSpin.src.gameplay_parts
{
    public record class GameStarterKit(
        List<BaseTileItem> TileItems,
        List<BaseRelic> Relics,
        TokensCollection TokensCollection,
        int DiamondsCount)
    {

        static public TokensCollection RandomTokensCollection(int tokensCount)
        {
            TokensCollection tC = new();
            for (int i = 0; i < tokensCount; i++)
            {
                tC.AddToken(TokensCollection.GetRandomTokenType());
            }
            return tC;
        }
    }
}
