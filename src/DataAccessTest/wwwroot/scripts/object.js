﻿var vueObject;
var vue_option = {
    el: '#work_space',
    data: {
        /**
         * API前缀
         */
        apiPrefix: '',
        ws_active: false,
        idField: "id",
        stateData: false,
        historyData: false,
        auditData: false,
        currentRow: null,
        list: {
            keyWords: null,
            dataState: -1,
            audit: -1,
            sort: null,
            order: null,
            rows: [],
            page: 1,
            pageSize: 15,
            pageSizes: [15, 20, 30, 50, 100],
            pageCount: 0,
            total: 0,
            multipleSelection: []
        },
        form: {
            readonly: false,
            visible: false,
            loading: false,
            edit: false,
            data: null,
            rules: null,
            types: null
        }
    },
    filters: {
        boolFormater(b) {
            return b ? "是" : "否";
        },
        formatTime(time) {
            return !time ? null : NewDate(time).format("yyyy-MM-dd hh:mm:ss");
        },
        formatDate(date) {
            return !date ? null : NewDate(date).format("yyyy-MM-dd");
        },
        emptyNumber(number) {
            if (number) {
                return number;
            } else {
                return "-";
            }
        },
        formatUnixDate(unix) {
            if (unix === 0)
                return "";
            return new Date(unix * 1000).format("MM-dd hh:mm:ss");
        },
        formatNumber(number) {
            if (number) {
                return number.toFixed(4);
            } else {
                return "0.0";
            }
        },
        formatNumber2(number) {
            if (number) {
                return number.toFixed(2);
            } else {
                return "0.0";
            }
        },
        thousandsNumber(number) {
            if (number) {
                return toThousandsInt(number);
            } else {
                return "0";
            }
        },
        formatMoney(number) {
            if (number) {
                return "￥" + number.toFixed(2);
            } else {
                return "￥0.00";
            }
        },
        formatNumber1(number) {
            if (number) {
                return number.toFixed(1);
            } else {
                return "0.0";
            }
        },
        formatNumber0(number) {
            if (number) {
                return number.toFixed(0);
            } else {
                return "0";
            }
        },
        formatHex(number) {
            if (number) {
                return number.toString(16).toUpperCase();
            } else {
                return "-";
            }
        },
        dataStateIcon(dataState) {
            switch (dataState) {
                case "None":
                    return "el-icon-edit";
                case "Enable":
                    return "el-icon-video-play";
                case "Disable":
                    return "el-icon-video-pause";
                case "Orther":
                    return "el-icon-user";
                case "Lock":
                    return "icon_a_end";
                case "Discard":
                    return "el-icon-delete";
                case "Delete":
                    return "el-icon-close";
            }
            return "el-icon-question";
        },
        typeIcon(val) {
            switch (val) {
                case 'App': return 'el-icon-mobile-phone';
                case 'Root': return 'el-icon-menu';
                case 'Folder': return 'el-icon-folder-opened';
                case 'Page': return 'el-icon-document';
                case 'Button': return 'el-icon-mouse';
                case 'Action': return 'el-icon-mouse';
                case 'Position': return 'el-icon-user';
                case 'Person': return 'el-icon-s-custom';
                case 'Organization': return 'el-icon-s-shop';
                case 'Area': return 'el-icon-office-building';
                case 'Department': return 'el-icon-user';
                default: 
                    return 'el-icon-view';
            }
        }
    },
    methods: {
        goHome() {
            showIframe('/home.html');
        },
        doQuery() {
            vue_option.data.list.rows = [];
            vue_option.data.list.page = 0;
            vue_option.data.list.pageCount = 0;
            vue_option.data.list.total = 0;
            this.loadList();
        },
        loadList(callback) {
            var that = this;
            var arg = this.getQueryArgs();
            ajax_post("载入列表数据",
                `${vue_option.data.apiPrefix}/edit/list`,
                arg,
                function (result) {
                    if (result.success) {
                        that.onListLoaded(result.data);
                        if (typeof callback === "function")
                            callback(result.data);
                        else {
                            vue_option.data.list.rows = result.data.rows;
                            vue_option.data.list.page = result.data.page;
                            vue_option.data.list.pageSize = result.data.pageSize;
                            vue_option.data.list.pageCount = result.data.pageCount;
                            vue_option.data.list.total = result.data.total;
                        }
                    }
                    else {
                        showStatus(result);
                    }
                });
        },
        getQueryArgs() {
            return {
                _field_: vue_option.data.list.field,
                _value_: vue_option.data.list.keyWords, 
                _state_: vue_option.data.list.dataState,
                _audit_: vue_option.data.list.audit,
                _page_: vue_option.data.list.page,
                _size_: vue_option.data.list.pageSize,
                _sort_: vue_option.data.list.sort,
                _order_: vue_option.data.list.order
            };
        },
        onListLoaded(data) {
            if (data.rows) {
                for (var idx = 0; idx < data.rows.length; idx++) {
                    var row = data.rows[idx];
                    if (!row.selected)
                        row.selected = false;
                    this.checkListData(row);
                }
            }
            else {
                data.rows = [];
            }
        },
        checkListData(row) {
        },
        dblclick(row, column, event) {
            this.currentRow = row;
            this.doEdit();
        },
        sizeChange(size) {
            vue_option.data.list.pageSize = size;
            this.loadList();
        },
        pageChange(page) {
            vue_option.data.list.page = page;
            this.loadList();
        },
        onSort(arg) {
            vue_option.data.list.sort = arg.prop;
            vue_option.data.list.order = arg.order === "ascending" ? "asc" : "desc";
            this.loadList();
        },
        currentRowChange(row) {
            this.currentRow = row;
        },
        selectionRowChange(val) {
            this.list.multipleSelection = val;
        },
        doAddNew() {
            var data = this.form;
            data.data = this.getDef();
            if (!data.data)
                return;
            data.readonly = false;
            data.edit = false;
            data.visible = true;
        },
        getDef() {
            return {};
        },
        doEdit() {
            if (!this.currentRow) {
                common.showError('编辑', '请单击一行');
                return;
            }
            var data = this.form;
            data.data = this.currentRow;
            data.readonly = this.checkReadOnly(data.data);
            data.edit = true;
            data.visible = true;
        },
        checkReadOnly(row) {
            return !row || row.isFreeze;
        },
        save() {
            var that = this;
            var data = that.form;
            this.$refs['dataForm'].validate((valid) => {
                if (!valid) {
                    common.showError('编辑', '数据校验出错误，请修正后再保存');
                    return false;
                }
                var isEdit = data.edit;
                data.loading = true;
                ajax_post("保存数据", data.edit ? `${vue_option.data.apiPrefix}/edit/update?id=${data.data[that.idField]}` : `${vue_option.data.apiPrefix}/edit/addnew`, data.data,
                    function (result) {
                        data.loading = false;
                        if (result.success) {
                            common.showMessage('保存数据', '操作成功');
                            data.visible = false;
                            result.data.edit = isEdit;
                            that.onSaved(result.data);
                            that.loadList();
                        }
                    },
                    function (result) {
                        data.loading = false;
                        common.showError('保存数据', result && result.message ? result.message : '更新失败');
                    });
                return true;
            });
        },
        onSaved(data) {

        },
        doDelete() {
            var me = this;
            this.mulitSelectAction("删除", `${vue_option.data.apiPrefix}/edit/delete`, function (row) {
                if (!me.stateData)
                    return true;
                return row.dataState === 'None'
                    || row.dataState === 'Discard'
                    || row.dataState === 'Delete';
            }, ids => {
                me.onDeleted(ids);
            });
        },
        onDeleted(ids) {

        },
        handleDataCommand(command) {
            switch (command) {
                case "Disable": this.doDisable(); break;
                case "Discard": this.doDiscard(); break;
                case "Reset": this.doReset(); break;
                case "Lock": this.doLock(); break;
                case "Unlock": this.doUnlock(); break;
                case "Back": this.doBack(); break;
                case "Pass": this.doPass(); break;
                case "Deny": this.doDeny(); break;
                case "ReDo": this.doReDo(); break;
            }
        },
        doEnable() {
            this.mulitSelectAction("启用", `${vue_option.data.apiPrefix}/state/enable`, function (row) {
                return !row.IsFreeze
                    && row.dataState !== 'Enable'
                    && row.dataState !== 'Discard'
                    && row.dataState !== 'Delete';
            });
        },
        doDisable() {
            this.mulitSelectAction("禁用", `${vue_option.data.apiPrefix}/state/disable`, function (row) {
                return !row.IsFreeze
                    && row.dataState !== 'Disable'
                    && row.dataState !== 'Discard'
                    && row.dataState !== 'Delete';
            });
        },
        doDiscard() {
            this.mulitSelectAction("废弃", `${vue_option.data.apiPrefix}/state/discard`, function (row) {
                return !row.IsFreeze
                    && row.dataState === 'None';
            });
        },
        doReset() {
            this.mulitSelectAction("重置", `${vue_option.data.apiPrefix}/state/reset`);
        },
        doLock() {
            this.mulitSelectAction("锁定", `${vue_option.data.apiPrefix}/state/lock`, function (row) {
                return !row.IsFreeze;
            });
        },
        doUnlock() {
            this.mulitSelectAction("解锁", `${vue_option.data.apiPrefix}/state/unlock`, function (row) {
                return row.IsFreeze;
            });
        },
        doBack() {
            this.mulitSelectAction("退回", `${vue_option.data.apiPrefix}/audit/back`, function (row) {
                return !row.IsFreeze
                    && (row.auditState === 'Submit' || row.auditState === 'Pass');
            });
        },
        doPass() {
            this.mulitSelectAction("通过", `${vue_option.data.apiPrefix}/audit/pass`, function (row) {
                return !row.IsFreeze
                    && (row.auditState === 'Submit' || row.auditState === 'Pass');
            });
        },
        doDeny() {
            this.mulitSelectAction("否决", `${vue_option.data.apiPrefix}/audit/deny`, function (row) {
                return !row.IsFreeze
                    && (row.auditState === 'Submit' || row.auditState === 'Pass');
            });
        },
        doReDo() {
            this.mulitSelectAction("重新审核", `${vue_option.data.apiPrefix}/audit/redo`, function (row) {
                return row.auditState === 'Pass'
                    || row.auditState === 'Deny'
                    || row.auditState === 'End';
            });
        },
        getSelectedRows(filter) {
            var rows = this.list.multipleSelection;
            if (!rows || rows.length === 0)
                return null;
            var ids = null;
            for (var idx = 0; idx < rows.length; idx++) {
                if (!filter || filter(rows[idx])) {
                    if (!ids)
                        ids = `${rows[idx][this.idField]},`;
                    else
                        ids += `${rows[idx][this.idField]},`;
                }
                else { this.$refs.dataTable.toggleRowSelection(rows[idx], false); }
            }
            return ids;
        },
        mulitSelectAction(title, api, filter, callback, arg) {
            var that = this;
            var ids = this.getSelectedRows(filter);
            if (!ids) {
                that.$notify({
                    message: '请选择一或多行合格的数据,不适合当前操作的选择已被自动清除！',
                    duration: 1500,
                    type: 'error'
                });
                return;
            }
            vueObject.$confirm(`你确定要${title}所选择的数据吗?`, title, {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                if (!arg)
                    arg = { selects: ids };
                else
                    arg.selects = ids;
                ajax_post(title, api, arg, function (result) {
                    if (result.success) {
                        that.$notify({
                            message: result.message ? result.message : '操作成功',
                            type: 'success'
                        });
                        if (callback)
                            callback(arg.selects);
                        that.loadList();
                    }
                    else {
                        that.$notify({
                            message: result.message ? '操作失败:' + result.message : '操作失败',
                            type: 'error'
                        });
                    }
                }, function (result) {
                    that.$notify({
                        message: result && result.message ? '操作失败:' + result.message : '操作失败',
                        type: 'error'
                    });
                });
            });
        }
    }
};