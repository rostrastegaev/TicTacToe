const maxPagesCount = 5;
const pageSize = 8;

function Pagination(element) {
    var self = this;
    var handlers = [];
    var navigationHrefs = [];
    var renderedPages = [];
    var pagesContainer = null;
    var currentPage = 0;
    var pagesCount = 0;

    this.addHandler = function (handler) {
        handlers.push(handler);
    };

    this.refresh = function () {
        selectPage(currentPage);
    };

    function selectPage(pageNumber) {
        $.ajax({
            url: '/api/games?page=' + pageNumber + '&pageSize=' + pageSize,
            method: 'GET',
            success: function (response) {
                currentPage = response.page;
                pagesCount = response.pagesCount;
                addPageToSearch(currentPage);

                onPageChanged();
                renderPages();

                handlers.forEach(function (handler) {
                    handler(response);
                });
            }
        });
    }

    function renderPages() {
        renderedPages.forEach(function (page) {
            page.remove();
        });

        var left = 0;
        var right = 0;
        if (pagesCount < maxPagesCount) {
            left = 0;
            right = pagesCount - 1;
        } else {
            var tempPagesCount = maxPagesCount - 1;
            var temp = tempPagesCount / 2;
            if (currentPage - temp < 0) {
                left = 0;
                right = tempPagesCount;
            } else if (currentPage + temp >= pagesCount) {
                left = pagesCount - maxPagesCount;
                right = pagesCount - 1;
            } else {
                left = currentPage - temp;
                right = currentPage + temp;
            }
        }

        for (var i = right; i >= left; --i) {
            createPage(i);
        }
    }

    function createPage(pageNumber) {
        var page = $(pageTemplate(pageNumber));
        if (pageNumber === currentPage) {
            page.addClass('active');
        }
        page.click(function (event) {
            event.preventDefault();
            selectPage(pageNumber);
        });

        pagesContainer.after(page);
        renderedPages.push(page);
    }

    function pageTemplate(number) {
        return '<li class="page-item"><a class="page-link" href="#">' + (number + 1) + '</a></li>';
    }

    function onPageChanged() {
        checkPaginationState();
        navigationHrefs.forEach(function (href) {
            href.onPageChanged();
        });
    }

    function checkPaginationState() {
        if (pagesCount > 1) {
            element.show();
        } else {
            element.hide();
        }
    }

    function NavigationHref(element, pageCondition, onClick) {
        var self = this;
        this.element = element;
        this.pageCondition = pageCondition;
        this.onPageChanged = function () {
            if (self.pageCondition()) {
                self.element.addClass('disabled');
            } else {
                self.element.removeClass('disabled');
            }
        };
        this.element.click(function (event) {
            event.preventDefault();
            if (!self.pageCondition()) {
                onClick();
                self.onPageChanged();
            }
        });
    }

    function addPageToSearch(pageNumber) {
        var pageUrl = window.location.protocol + "//" +
            window.location.host + window.location.pathname + '?page=' + pageNumber;
        window.history.pushState({ path: pageUrl }, '', pageUrl);
    }

    function init() {
        checkPaginationState();
        var first = new NavigationHref($('.page-link[aria-label="First"]', element).parent(),
            function () { return currentPage === 0; },
            function () { selectPage(0); });
        navigationHrefs.push(first);

        var previous = new NavigationHref($('.page-link[aria-label="Previous"]', element).parent(),
            function () { return currentPage === 0; },
            function () { selectPage(currentPage - 1); });
        navigationHrefs.push(previous);
        pagesContainer = previous.element;

        var next = new NavigationHref($('.page-link[aria-label="Next"]', element).parent(),
            function () { return currentPage === pagesCount - 1; },
            function () { selectPage(currentPage + 1); });
        navigationHrefs.push(next);

        var last = new NavigationHref($('.page-link[aria-label="Last"]', element).parent(),
            function () { return currentPage === pagesCount - 1; },
            function () { selectPage(pagesCount - 1); });
        navigationHrefs.push(last);

        var pageParam = 'page=';
        var pageParamIndex = window.location.href.indexOf(pageParam);
        if (pageParamIndex !== -1) {
            var number = window.location.href.substring(pageParamIndex + pageParam.length);
            selectPage(+number);
        } else {
            selectPage(0);
        }
    }
    init();
}

export default Pagination;