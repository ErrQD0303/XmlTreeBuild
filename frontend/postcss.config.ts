import autoprefixer from "autoprefixer";

import cssnano from "cssnano";

import postcssNested from "postcss-nested";

export default {
  syntax: "postcss-scss",

  plugins: [
    postcssNested(), // Nested CSS should be processed first
    autoprefixer(), // Autoprefixer comes after nested processing
    cssnano({ preset: "default" }), // Minification comes last],
  ],
};
