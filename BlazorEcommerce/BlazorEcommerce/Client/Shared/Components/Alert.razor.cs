namespace BlazorEcommerce.Client.Shared.Components
{
    public partial class Alert : ComponentBase
    {
        #region Parameter

        /// <summary>
        /// The type of the Alert.
        /// <code>primary</code>
        /// <code>secondary</code>
        /// <code>success</code>
        /// <code>danger</code>
        /// <code>warning</code>
        /// <code>info</code>
        /// <code>light</code>
        /// <code>dark</code>
        /// </summary>
        [Parameter] public string? Type { get; set; } = string.Empty;

        /// <summary>
        /// If you want a full Alert or just a Message
        /// </summary>
        [Parameter] public bool IsFullBody { get; set; } = false;

        /// <summary>
        /// If <see cref="IsFullBody" /> Must include Title
        /// </summary>
        [Parameter] public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// If <see cref="IsFullBody" /> Must include Description
        /// </summary>
        [Parameter] public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// The Message to show in the Alert
        /// </summary>
        [Parameter] public string? Message { get; set; } = string.Empty;

        #endregion Parameter
    }
}
