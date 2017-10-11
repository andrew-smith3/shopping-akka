using System;

namespace ShoppingCart.Data.Projections
{
    public class CartProjection : Projection
    {
        public static string ProjectionType = nameof(CartProjection);

        public override string Type => ProjectionType;

        public Guid UserId { get; }

        public int Subtotal { get; }

        public CartProjection(Guid userId, int subtotal)
        {
            UserId = userId;
            Subtotal = subtotal;
        }
    }
}
