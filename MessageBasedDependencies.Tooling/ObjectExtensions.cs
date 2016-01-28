namespace MessageBasedDependencies.Tooling
{
    public static partial class ObjectExtensions
    {
        public static string GetObjectId(this object @this)
        {
            return @this.GetType().Name + @this.GetHashCode()%1000;
        }
    }
}
