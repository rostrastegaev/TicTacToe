import './games-list.scss';
import moment from 'moment';
import Pagination from './pagination';
const dateFormat = 'YYYY/MM/DD HH:mm';

function GamesList(table, pagination) {
    var self = this;
    var body = $('tbody', table);
    pagination.addHandler(renderTable);

    function renderTable(fetchResult) {
        $('tr', body).remove();
        if (fetchResult.totalCount === 0) {
            table.hide();
        } else {
            table.show();
            fetchResult.items.forEach(function (item) {
                var row = $(rowTemplate(item));
                row.click(function () {
                    var paramsIndex = window.location.href.indexOf('?');
                    var params = '';
                    if (paramsIndex !== -1) {
                        params = window.location.href.substring(paramsIndex);
                    }

                    window.location.href = item.id + params;
                });
                body.append(row);
            });
        }
    }

    function rowTemplate(game) {
        return '<tr class="games-list__game"><th scope="row">' + game.id + '</th>' +
            '<td>' + moment.utc(game.startedAt).local().format(dateFormat) + '</td>' +
            '<td>' + moment.utc(game.endedAt).local().format(dateFormat) + '</td></tr>';
    }
}

var gamesList = $('.games-list');
var pagination = new Pagination($('.pagination', gamesList));
var gamesTable = new GamesList($('.table', gamesList), pagination);

export { pagination };