using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Common;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class CreateTransactionPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateTransactionRequest InputModel { get; set; } = new();
    protected List<ComboItens> CategoriesCombo { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;

    [Inject]
    public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetCombosRequest();
            var result = await CategoryHandler.GetAllComboSelectAsync(request);
            if (result.IsSuccess)
            {
                CategoriesCombo = result.Data ?? [];
                InputModel.CategoryId = long.Parse(CategoriesCombo.FirstOrDefault()?.Value ?? "0");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message ?? "Lançamento criado com sucesso", Severity.Success);
                NavigationManager.NavigateTo("/transactions");
            }
            else
                Snackbar.Add(result.Message ?? "Não foi possível criar o lançamento", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}