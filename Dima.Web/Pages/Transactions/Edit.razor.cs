using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Common;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = true;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    protected List<ComboItens> CategoriesCombos { get; set; } = [];

    #endregion
    
    #region Parameters

    [Parameter]
    public string Id { get; set; } = string.Empty;

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

    protected override async Task OnParametersSetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            Snackbar.Add("Parâmetro inválido", Severity.Error);
            return;
        }
        
        IsBusy = true;

        await GetTransactionByIdAsync();
        await GetCategoriesAsync();

        IsBusy = false;
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add("Lançamento atualizado", Severity.Success);
                NavigationManager.NavigateTo("/transactions");
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
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

    #region Private Methods

    private async Task GetTransactionByIdAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransactionByIdRequest { Id = long.Parse(Id) };
            var result = await TransactionHandler.GetByIdAsync(request);
            if (result is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateTransactionRequest
                {
                    CategoryId = result.Data.CategoryId,
                    PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                    Title = result.Data.Title,
                    Type = result.Data.Type,
                    Amount = Math.Abs(result.Data.Amount),
                    Id = result.Data.Id,
                };
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

    private async Task GetCategoriesAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetCombosRequest();
            var result = await CategoryHandler.GetAllComboSelectAsync(request);
            if (result.IsSuccess)
            {
                CategoriesCombos = result.Data ?? [];
                InputModel.CategoryId = long.Parse(CategoriesCombos.FirstOrDefault()?.Value ?? "0");
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
}

