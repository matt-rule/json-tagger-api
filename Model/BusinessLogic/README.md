Methods should not use global state.
Methods should use => syntax. When defining local variables or using try-catches, do this:
    `public static Func<char, int> CharToValue = (c) => {`
        `...`
    `}`
