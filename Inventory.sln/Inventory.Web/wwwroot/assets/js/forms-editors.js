/**
 * Form Editors
 */

'use strict';

(function () {
  // Snow Theme
    // --------------------------------------------------------------------
    if (document.querySelector('#snow-editor')) {
        const snowEditor = new Quill('#snow-editor', {
            bounds: '#snow-editor',
            modules: {
                formula: true,
                toolbar: '#snow-toolbar'
            },
            theme: 'snow'
        });
    }


  // Bubble Theme
    // --------------------------------------------------------------------
    if (document.querySelector('#bubble-editor')) {
        const bubbleEditor = new Quill('#bubble-editor', {
            modules: {
                toolbar: '#bubble-toolbar'
            },
            theme: 'bubble'
        });
    }


  // Full Toolbar
  // --------------------------------------------------------------------
  const fullToolbar = [
    [
      {
        font: []
      },
      {
        size: []
      }
    ],
    ['bold', 'italic', 'underline', 'strike'],
    [
      {
        color: []
      },
      {
        background: []
      }
    ],
    [
      {
        script: 'super'
      },
      {
        script: 'sub'
      }
    ],
    [
      {
        header: '1'
      },
      {
        header: '2'
      },
      'blockquote',
      'code-block'
    ],
    [
      {
        list: 'ordered'
      },
      {
        list: 'bullet'
      },
      {
        indent: '-1'
      },
      {
        indent: '+1'
      }
    ],
    [{ direction: 'rtl' }],
    ['link', 'image', 'video', 'formula'],
    ['clean']
    ];
    if (document.querySelector('#full-editor')) {
        var fullEditor = new Quill('#full-editor', {
            bounds: '#full-editor',
            placeholder: 'Type Something...',
            modules: {
                formula: true,
                toolbar: fullToolbar
            },
            theme: 'snow'
        });
    }
   
})();
