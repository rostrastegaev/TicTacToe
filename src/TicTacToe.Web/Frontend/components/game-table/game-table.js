import './game-table.scss';
import octicons from 'octicons';
import { pagination } from '../games-list/games-list';

const tableSize = 3;
function markTemplate(mark) {
    return '<div class="game__mark game__mark_' + mark + '"></div>';
}

function GameTable(element) {
    var self = this;
    var cells = [];
    var handlers = [];
    this.userMark = 'x';

    this.addHandler = function (handler) {
        handlers.push(handler);
    };

    this.createGame = function () {
        self.userMark = 'o';
        createGame(function (response) {
            processTurnInfo(response.history[0]);
        }, false);
    };

    function onCellClicked(cell) {
        if (element.hasClass('game_shake') || cells.some(function (c) { return c.isShaking(); })) {
            cells.forEach(function (c) {
                c.clear();
            });
            element.removeClass('game_shake');
            onGameCleared();
            return;
        }

        if (!cell.isEmpty()) {
            return;
        }

        if (cells.every(function (c) { return c.isEmpty(); })) {
            self.userMark = 'x';
            createGame(function (response) {
                makeTurn(cell);
            }, true);
        } else {
            makeTurn(cell);
        }
    }

    function onGameCleared() {
        pagination.refresh();
        handlers.forEach(function (handler) {
            if (handler.onGameCleared) {
                handler.onGameCleared();
            }
        });
    }

    function onGameStarted() {
        handlers.forEach(function (handler) {
            if (handler.onGameStarted) {
                handler.onGameStarted();
            }
        });
    }

    function onGameEnded(turnInfo) {
        handlers.forEach(function (handler) {
            if (handler.onGameEnded) {
                handler.onGameEnded(turnInfo);
            }
        });
    }

    function createGame(success, playerFirst) {
        onGameStarted();
        $.ajax({
            url: '/api/games',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ isPlayerFirst: playerFirst }),
            success: function (response) {
                success(response);
            }
        });
    }

    function makeTurn(cell) {
        cell.mark(self.userMark);
        $.ajax({
            url: '/api/turns',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ x: cell.x, y: cell.y }),
            success: function (response) {
                processTurnInfo(response);
            }
        });
    }

    function processTurnInfo(turnInfo) {
        cells.forEach(function (cell) {
            if (cell.x === turnInfo.cell.x && cell.y === turnInfo.cell.y) {
                cell.mark(turnInfo.mark === 0 ? 'x' : 'o');
            }
        });

        if (turnInfo.gameState.isEnded) {
            onGameEnded(turnInfo);

            if (turnInfo.gameState.winningCells) {
                cells.forEach(function (cell) {
                    turnInfo.gameState.winningCells.forEach(function (winningCell) {
                        if (cell.x === winningCell.x && cell.y === winningCell.y) {
                            cell.shake();
                        }
                    });
                });
            } else {
                element.addClass('game_shake');
            }
        }
    }

    function Cell(element, x, y) {
        var self = this;
        this.x = x;
        this.y = y;
        this.element = element;

        this.isEmpty = function () {
            return $('.game__mark', element).length === 0;
        };

        this.isShaking = function () {
            return $('.game__mark', element).hasClass('game__mark_shake');
        };

        this.clear = function () {
            $('.game__mark', element).remove();
        };

        this.mark = function (mark) {
            element.append(markTemplate(mark));
        };

        this.shake = function () {
            var mark = $('.game__mark', element);
            mark.addClass('game__mark_shake');
        };        

        function init() {
            element.click(function () {
                onCellClicked(self);
            });
        }
        init();
    }
    
    this.init = function () {
        $('.game__cell', element).each(function (index) {
            var x = Math.floor(index / tableSize);
            var y = index % tableSize;
            cells.push(new Cell($(this), x, y));
        });
        var mark = element.attr('data-mark');
        if (mark) {
            self.userMark = mark;
        }
        if (+element.attr('data-id')) {
            onGameStarted();
        }
    };
}

function GameTableViewOnly(element) {
    var self = this;
    var cells = [];
    var handlers = [];
    var game = null;
    var currentTurn = -1;

    this.addHandler = function (handler) {
        handlers.push(handler);
    }

    this.next = function () {
        ++currentTurn;
        var cell = getCell(game.history[currentTurn]);
        cell.mark(game.history[currentTurn].mark === 0 ? 'x' : 'o');
        onTurnChanged();
    }

    this.previous = function () {
        var cell = getCell(game.history[currentTurn]);
        cell.unmark();
        --currentTurn;
        onTurnChanged();
    };

    function getCell(turnInfo) {
        for (var i = 0; i < cells.length; ++i) {
            if (turnInfo.cell.x === cells[i].x &&
                turnInfo.cell.y === cells[i].y) {
                return cells[i];
            }
        }
    }

    function onTurnChanged() {
        handlers.forEach(function (handler) {
            handler.checkNext(currentTurn < game.history.length - 1);
            handler.checkPrevious(currentTurn > 0);
        });
    }

    function Cell(element, x, y) {
        var self = this;
        this.x = x;
        this.y = y;

        this.mark = function (mark) {
            element.append(markTemplate(mark));
        };

        this.unmark = function () {
            $('.game__mark', element).remove();
        }
    }

    this.init = function () {
        $('.game__cell', element).each(function (index) {
            var x = Math.floor(index / tableSize);
            var y = index % tableSize;
            cells.push(new Cell($(this), x, y));
        });
        game = JSON.parse(element.attr('data-object'));
        self.next();
    };
}

var game = $('.game');
if (game.attr('data-mode') == 'view') {
    var gameTable = new GameTableViewOnly(game);
    var previousArrow = $('.game__previous');
    previousArrow.append(octicons['arrow-left'].toSVG({ height: 64, width: 64 }));
    var nextArrow = $('.game__next');
    nextArrow.append(octicons['arrow-right'].toSVG({ height: 64, width: 64 }));

    gameTable.addHandler({
        checkNext: function (condition) {
            if (condition) {
                nextArrow.css('visibility', 'visible');
            } else {
                nextArrow.css('visibility', 'hidden');
            }
        },
        checkPrevious: function (condition) {
            if (condition) {
                previousArrow.css('visibility', 'visible');
            } else {
                previousArrow.css('visibility', 'hidden');
            }
        }
    });
    gameTable.init();

    previousArrow.click(function () {
        gameTable.previous();
    });
    nextArrow.click(function () {
        gameTable.next();
    });

} else {
    var gameTable = new GameTable(game);
    var computerButton = $('.game__computer');
    computerButton.append(octicons['device-desktop'].toSVG({ height: 64, width: 64 }));
    var messageContainer = $('.game__message');

    computerButton.click(function () {
        gameTable.createGame();
    });

    gameTable.addHandler({
        onGameCleared: function () {
            computerButton.show();
        },
        onGameStarted: function () {
            computerButton.hide();
        }
    });
    gameTable.addHandler({
        onGameEnded: function (turnInfo) {
            messageContainer.show();
            if (turnInfo.gameState.winningCells) {
                if ((turnInfo.gameState.winner == 0 ? 'x' : 'o') == gameTable.userMark) {
                    messageContainer.text('User win!');
                } else {
                    messageContainer.text('Computer win!');
                }
            } else {
                messageContainer.text('Draw');
            }
        },
        onGameCleared: function () {
            messageContainer.hide();
        },
        onGameStarted: function () {
            messageContainer.hide();
        }
    });
    gameTable.init();
}