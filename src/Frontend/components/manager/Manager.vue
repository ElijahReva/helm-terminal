<template lang="html">
    <div class="ui grid container">
        <ActionMenu/>
        <ChartList />                        
    </div>       
</template>

<script>
import JQuery from 'jquery'
import ActionMenu from "./ActionMenu";
import ChartList from "./ChartList";
let $ = JQuery;

export default {
    name: 'Main',
    components: {ChartList, ActionMenu},
    mounted(){      
        $('.overlay').visibility({
          type: 'fixed',
          offset: 80
        })
  }
}
</script>

<style scoped>
.overlay {
  float: left;
  margin: 0 3em 1em 0;
}
.overlay .menu {
  position: relative;
  left: 0;
  transition: left 0.5s ease;
}
.overlay.fixed .menu {
  left: 800px;
}

.text.container .left.floated.image {
  margin: 2em 2em 2em -4em;
}
.text.container .right.floated.image {
  margin: 2em -4em 2em 2em;
}
</style>