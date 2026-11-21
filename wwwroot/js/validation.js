(() => {
    if (!window.jQuery || !jQuery.validator || !jQuery.validator.unobtrusive) return;

    jQuery.validator.addMethod('strongpassword', (value, element, allowEmpty) => {
        if (!value) return true;
        return value.length >= 8
            && /[A-Z]/.test(value)
            && /[a-z]/.test(value)
            && /\d/.test(value)
            && /[^A-Za-z0-9]/.test(value);
    });

    jQuery.validator.unobtrusive.adapters.add('strongpassword', options => {
        options.rules.strongpassword = true;
        options.messages.strongpassword = options.message;
    });
})();
