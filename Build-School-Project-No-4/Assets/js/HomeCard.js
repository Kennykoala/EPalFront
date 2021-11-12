//Create ProductCards
function CreateProducts(item) {
    let card = document.querySelector('#cardCreator')
    let cloneContent = card.content.cloneNode(true);
    cloneContent.querySelector('.item-Card a').href = '@Url.Action("Detail", new {id = "ID"})'.replace("ID", item.ProductId);
    cloneContent.querySelectorAll('.user-Title-Item span')[0].innerText = item.Rank;
    cloneContent.querySelectorAll('.user-Title-Item span')[1].innerText = item.Position;
    cloneContent.querySelector('.head-Person-Pic img').src = item.CreatorImg;
    cloneContent.querySelector('.status-Pic img').src = item.LineStatus;
    cloneContent.querySelector('.user-Info-Name span').innerText = item.CreatorName;
    cloneContent.querySelector('.price-Span').innerText = item.UnitPrice;
    row.appendChild(cloneContent);
}

