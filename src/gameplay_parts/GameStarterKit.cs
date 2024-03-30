using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts
{
    public record class GameStarterKit(
        List<TileItem> TileItems,
        List<Relic> Relics,
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
