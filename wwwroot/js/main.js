window.blazorDialogFunctions = {
    openDialog: function(dialogId) {
        document.getElementById(dialogId).showModal();
    },
    closeDialog: function(dialogId) {
        document.getElementById(dialogId).close();
    }
};