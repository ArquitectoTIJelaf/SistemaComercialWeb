const app = new Vue({
    el: '#app',
    data: {
        titulo: 'Introducción desde Vue.js',
        frutas: [
            { nombre: 'Pera', cantidad: 12, estado: true },
            { nombre: 'Manzana', cantidad: 2, estado: true },
            { nombre: 'Durazno', cantidad: 0, estado: true },
            { nombre: 'Platano', cantidad: 20, estado: true },
            { nombre: 'Naranja', cantidad: 0, estado: false }
        ]
    }
});