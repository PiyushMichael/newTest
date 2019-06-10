import React from 'react';
import ReactDOM from 'react-dom';
//import Popup from 'reactjs-popup';

const strElement = <div><h1>Hello React! H1 element added</h1> <li>peugeot</li> <li>citreon</li> <li>renault</li></div>

class Garage extends React.Component
{
	render()
	{
		return <h1>this is the garage class inside car class</h1>;
	}
	componentWillUnmount()
	{
		alert("garage component unmounted");
	}
}

class Car extends React.Component
{
	constructor(props)
	{
		super(props);
		this.state = {bleh: "red", col: "EHEH", model: props.col.model, show: true};
		this.innerHTML = "inner HTML text :O"
	};
	changeColor =() =>{
		if(this.state.bleh == "red")this.setState({bleh: "BLUE"});
		if(this.state.bleh == "BLUE")this.setState({bleh: "red"});
	}
	delThis = () => {
		this.setState({show: false});
	}
	/*static getDerivedStateFromProps(props,state)
	{
		return {model: props.col.model};
	}*/
	Aler = () => {
		alert("teehee an alert box :)");
	}
	render()
	{
		let x;
		if(this.state.show == true)
		{
			x=<Garage />
		}
		return (
			<div>
				{x}
				<p>this paragraph of {this.state.bleh} car eheh</p>
				<p>second para {this.innerHTML}</p>
				<p>passing car details as a prop: {this.props.col.brand}, {this.props.col.model}</p>
				<p>same variable name as prop but in state {this.state.col}</p>
				<p>details through getDerivedStateFromProps: {this.state.bleh}, {this.state.col}, <b>{this.state.model}</b></p>
				<br/>
				<p>issa {this.state.bleh} color car</p>
				<button type="button" onClick={this.changeColor}>change colour</button>
				<button type="button" onClick={this.delThis}>delete/unmount garage component</button>
				<button type="button" onClick={this.Aler}>Shows Alert Box</button>
				<div id="boo"></div>
				<div id="deb"></div>
				<div id="up"></div>
			</div>
		);
	}
	componentDidMount(){
		setTimeout(() => {this.setState({model: 2000})}, 2000); //setState gets overridden by getDerivedStateFromProps. pass props in constructor instead
		document.getElementById("deb").innerHTML = "componentDidMount executed";
	}
	getSnapshotBeforeUpdate(prevProps,prevState)
	{
		document.getElementById("boo").innerHTML = "before the update value was: "+prevState.bleh;
	}
	componentDidUpdate()
	{
		document.getElementById("up").innerHTML += "component updated<br>"
	}
}
class App extends React.Component
{
	
	render()
	{
		const cars = {brand: "Ford", model: 1998};
		return(ReactDOM.render(<Car col={cars}/>,document.getElementById('root')))
	}
}

export default App;

